#region Usings

using System.Collections;

#endregion

namespace ZhEaIsNsAaBn.Utilities;

/// <summary>
/// Represents the outcome of an operation that can either succeed with a typed result or fail with errors.
/// </summary>
/// <typeparam name="TResult">Type of the operation result payload.</typeparam>
public class LayerResponse< TResult > : LayerResponse
{
    /// <summary>
    /// Initializes a new instance of <see cref="LayerResponse{TResult}"/> with a single error.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="error">The error associated with the response.</param>
    /// <param name="result">The result payload.</param>
    protected LayerResponse( bool isSuccess, Error error, TResult result = default! ) : base( isSuccess, error ) =>
        Result = result;

    /// <summary>
    /// Initializes a new instance of <see cref="LayerResponse{TResult}"/> with multiple errors.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="errors">The errors associated with the response.</param>
    /// <param name="result">The result payload.</param>
    protected LayerResponse( bool isSuccess, List<Error> errors, TResult result = default! ) :
        base( isSuccess, errors ) =>
        Result = result;

    /// <summary>
    /// Gets the operation result payload.
    /// </summary>
    public TResult? Result { get; }

    /// <summary>
    /// Gets a value indicating whether the response failed or contains an empty/default result.
    /// </summary>
    public bool IsFailureOrEmpty =>
        IsFailure || Equals( Result, default(TResult) ) || Result is ICollection { Count: 0 };

    /// <summary>
    /// Creates a failed response from a single error.
    /// </summary>
    /// <param name="error">The failure error.</param>
    /// <param name="result">Optional result payload.</param>
    /// <returns>A failed <see cref="LayerResponse{TResult}"/> instance.</returns>
    public static LayerResponse<TResult> Failure( Error error, TResult result = default! ) => new(false, error, result);

    /// <summary>
    /// Creates a failed response from multiple errors.
    /// </summary>
    /// <param name="errors">The failure errors.</param>
    /// <param name="result">Optional result payload.</param>
    /// <returns>A failed <see cref="LayerResponse{TResult}"/> instance.</returns>
    public static LayerResponse<TResult> Failure( List<Error> errors, TResult result = default! ) =>
        new(false, errors, result);

    /// <summary>
    /// Creates a failed response from a non-generic response.
    /// </summary>
    /// <param name="layerResponse">Source response.</param>
    /// <param name="result">Optional result payload.</param>
    /// <returns>A failed <see cref="LayerResponse{TResult}"/> instance.</returns>
    public static LayerResponse<TResult> Failure( LayerResponse layerResponse, TResult result = default! ) =>
        new(false, layerResponse.Errors, result);

    /// <summary>
    /// Creates a failed response from another generic response.
    /// </summary>
    /// <typeparam name="TOtherResult">Source result type.</typeparam>
    /// <param name="layerResponse">Source response.</param>
    /// <param name="result">Optional result payload.</param>
    /// <returns>A failed <see cref="LayerResponse{TResult}"/> instance.</returns>
    public static LayerResponse<TResult> Failure< TOtherResult >(
            LayerResponse<TOtherResult> layerResponse, TResult result = default!
        ) =>
        new(false, layerResponse.Errors, result);

    /// <summary>
    /// Converts a result value into a successful response.
    /// </summary>
    /// <param name="result">Result value to wrap.</param>
    public static implicit operator LayerResponse<TResult>( TResult? result ) =>
        result is null ? Success() : Success( result );

    /// <summary>
    /// Extracts the result payload from a response.
    /// </summary>
    /// <param name="result">The source response.</param>
    public static implicit operator TResult( LayerResponse<TResult?> result ) => result.Result!;

    /// <summary>
    /// Converts an <see cref="Error"/> into a failed response.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator LayerResponse<TResult>( Error error ) => Failure( error );

    /// <summary>
    /// Converts a list of <see cref="Error"/> values into a failed response.
    /// </summary>
    /// <param name="errors">The errors to convert.</param>
    public static implicit operator LayerResponse<TResult>( List<Error> errors ) => Failure( errors );

    /// <summary>
    /// Converts an <see cref="Exception"/> into a failed response using <see cref="Error.Unknown(Exception, IReadOnlyDictionary{string, object}?)"/>.
    /// </summary>
    /// <param name="exception">The exception to convert.</param>
    public static implicit operator LayerResponse<TResult>( Exception exception ) =>
        Failure( Error.Unknown( exception ) );

    /// <summary>
    /// Extracts the first error from a response.
    /// </summary>
    /// <param name="response">The source response.</param>
    /// <returns>The first error, or <see cref="Error.None"/> when no errors are available.</returns>
    public static implicit operator Error( LayerResponse<TResult> response ) => response.Errors.FirstOrDefault() ?? Error.None;

    /// <summary>
    /// Extracts the full error list from a response.
    /// </summary>
    /// <param name="response">The source response.</param>
    /// <returns>The response errors.</returns>
    public static implicit operator List<Error>( LayerResponse<TResult> response ) => response.Errors;

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    /// <param name="result">Optional result payload.</param>
    /// <returns>A successful <see cref="LayerResponse{TResult}"/> instance.</returns>
    public static LayerResponse<TResult> Success( TResult result = default! ) => new(true, Error.None, result);

    /// <summary>
    /// Converts a non-generic response into a generic response of a different result type.
    /// </summary>
    /// <typeparam name="TResult2">Target result type.</typeparam>
    /// <param name="result">The response to convert.</param>
    /// <returns>A converted response that preserves success/failure state.</returns>
    public static new LayerResponse<TResult2> ToLayerResponse< TResult2 >( LayerResponse result ) =>
        result.IsSuccess ? LayerResponse<TResult2>.Success() : LayerResponse<TResult2>.Failure( result.Errors );

    /// <summary>
    /// Converts a non-generic response into a non-generic response instance.
    /// </summary>
    /// <param name="result">The response to convert.</param>
    /// <returns>A converted response that preserves success/failure state.</returns>
    public static LayerResponse ToLayerResponse( LayerResponse result ) =>
        result.IsSuccess ? LayerResponse.Success() : LayerResponse.Failure( result.Errors );
}
