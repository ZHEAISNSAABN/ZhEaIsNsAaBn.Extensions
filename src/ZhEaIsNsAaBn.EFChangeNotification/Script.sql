USE [NewFeeds_09]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  MessageType [CaseIndexSummaryNotification]    Script Date: 9/3/2020 1:18:38 PM ******/
CREATE MESSAGE TYPE [CaseIndexSummaryNotification] VALIDATION = WELL_FORMED_XML
CREATE CONTRACT [CaseIndexSummaryNotification] ([CaseIndexSummaryNotification] SENT BY INITIATOR)
alter AUTHORIZATION ON CONTRACT::[CaseIndexSummaryNotification] TO [dbo] 
CREATE QUEUE [dbo].[CaseIndexSummaryNotificationSender] WITH STATUS = ON , RETENTION = OFF , POISON_MESSAGE_HANDLING (STATUS = ON) 
CREATE SERVICE [CaseIndexSummaryNotificationSender]  ON QUEUE [dbo].[CaseIndexSummaryNotificationSender]



CREATE TABLE [dbo].[CaseIndexSummaryNotificationReceivers](
	[Ident] [uniqueidentifier] NOT NULL,
	[DialogHandle] [uniqueidentifier] NULL,
	[Expires] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Ident] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE PROCEDURE [dbo].[RemoveExpiredCaseIndexSummaryConversations]
AS
BEGIN
	DECLARE @nowDate DATETIME = GETUTCDATE()
	DECLARE @count INT
	SELECT @count = count(*) FROM [CaseIndexSummaryNotificationReceivers] WHERE Expires <= @nowDate
	IF @count > 0 BEGIN
		--PRINT 'No expired conversations to remove'
	--END
	--ELSE BEGIN
		DECLARE @ident UNIQUEIDENTIFIER
		DECLARE @dialogHandle UNIQUEIDENTIFIER
		DECLARE enumCursor CURSOR FAST_FORWARD FOR
			SELECT Ident, DialogHandle FROM [CaseIndexSummaryNotificationReceivers] WHERE Expires <= @nowDate
		OPEN enumCursor
		FETCH NEXT FROM enumCursor INTO @ident, @dialogHandle
		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @fullName NVARCHAR(55)
			SET @fullName =  cast(@ident as NVARCHAR(55))
			--PRINT 'Removing expired conversation ' + @fullName
			END CONVERSATION @dialogHandle
			EXECUTE ('DROP SERVICE [' + @fullName + ']')
			EXECUTE ('DROP QUEUE [' + @fullName + ']')
			FETCH NEXT FROM enumCursor INTO @ident, @dialogHandle
		END
		CLOSE enumCursor
		DEALLOCATE enumCursor

		--PRINT 'Deleting expired conversation(s)';
		DELETE FROM [CaseIndexSummaryNotificationReceivers] WHERE Expires <= @nowDate
	END
END

CREATE PROCEDURE [dbo].[SendCaseIndexSummaryUpdate]
(
	@messageBody XML
)
AS
BEGIN
	-- Remove expired conversations
	EXEC [RemoveExpiredCaseIndexSummaryConversations]
	DECLARE @dialogHandle UNIQUEIDENTIFIER;
	DECLARE enumCursor CURSOR FAST_FORWARD FOR SELECT DialogHandle from [CaseIndexSummaryNotificationReceivers]
	OPEN enumCursor
	FETCH NEXT FROM enumCursor into @dialogHandle
	WHILE @@FETCH_STATUS = 0
	BEGIN
		;SEND ON CONVERSATION @dialogHandle MESSAGE TYPE [CaseIndexSummaryNotification](@messageBody);
		FETCH NEXT FROM enumCursor INTO @dialogHandle
	END
	CLOSE enumCursor
	DEALLOCATE enumCursor
END

CREATE TRIGGER [AFFeeds].[CaseIndexSummaryNotificationInsert]
ON [AFFeeds].[CaseIndexSummary] AFTER INSERT
AS
BEGIN
	DECLARE @messageBody XML, @ID int
	
  
	SELECT @messageBody = ( SELECT  
		*
		FROM  inserted
							 FOR XML PATH ('Inserted'))

	EXEC [SendCaseIndexSummaryUpdate] @messageBody
END
CREATE TRIGGER [AFFeeds].[CaseIndexSummaryNotificationUpdate]
ON [AFFeeds].[CaseIndexSummary] AFTER UPDATE
AS
BEGIN
	DECLARE @messageBody XML, @ID int
	
  
	SELECT @messageBody = ( SELECT  
		*
		FROM  inserted
							 FOR XML PATH ('Updated'))

	EXEC [SendCaseIndexSummaryUpdate] @messageBody
END



create PROCEDURE [dbo].[BeginCaseIndexSummaryConversation]
(
	@ident UNIQUEIDENTIFIER,
	@conversationTimeout int,
	@fullName NVARCHAR(55) OUT
)
AS
BEGIN
	SET @fullName = cast(@ident as NVARCHAR(55))
	DECLARE @dialogHandle UNIQUEIDENTIFIER
	SELECT @dialogHandle = DialogHandle FROM [CaseIndexSummaryNotificationReceivers] WHERE Ident = @ident
	IF @dialogHandle is not null
		UPDATE [CaseIndexSummaryNotificationReceivers] SET Expires = DATEADD(SECOND, @conversationTimeout, GETUTCDATE()) WHERE Ident = @ident
	ELSE
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION
				EXECUTE ('CREATE QUEUE [' + @fullName + ']')
				EXECUTE ('CREATE SERVICE [' + @fullName + '] ON QUEUE [' + @fullName + '] ([CaseIndexSummaryNotification])')
				BEGIN DIALOG CONVERSATION @dialogHandle
					FROM SERVICE [CaseIndexSummaryNotificationSender] TO SERVICE @fullName ON CONTRACT [CaseIndexSummaryNotification] WITH ENCRYPTION = OFF;
				INSERT INTO [CaseIndexSummaryNotificationReceivers] VALUES (@ident, @dialogHandle, DATEADD(second, @conversationTimeout, GETUTCDATE()) );
			COMMIT TRANSACTION
			--PRINT 'Created queue and service ' + @fullName
			--PRINT 'Started dialog conversation ' + cast(@dialogHandle as NVARCHAR(55))
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;
			--EXECUTE [dbo].[LogError];
			DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT
			SELECT  @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
		END CATCH;
	END
END