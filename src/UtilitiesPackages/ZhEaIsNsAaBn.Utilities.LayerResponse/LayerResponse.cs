namespace ZhEaIsNsAaBn.Utilities;

/// <summary>
/// Represents the outcome of an operation that can either succeed or fail with one or more errors.
/// </summary>
public class LayerResponse
{
    /// <summary>
    /// Initializes a new instance of <see cref="LayerResponse"/> with a single error.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="error">The error associated with the response.</param>
    /// <exception cref="InvalidOperationException">Thrown when success/failure state does not match the error value.</exception>
    protected LayerResponse( bool isSuccess, Error error )
    {
        if ( isSuccess && error != Error.None )
        {
            throw new InvalidOperationException();
        }

        if ( !isSuccess &&
             error == Error.None )
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;

        if ( error != Error.None )
        {
            Errors.Add( error );
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="LayerResponse"/> with multiple errors.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="errors">The errors associated with the response.</param>
    /// <exception cref="InvalidOperationException">Thrown when success/failure state does not match the errors collection.</exception>
    protected LayerResponse( bool isSuccess, List<Error>? errors = null )
    {
        errors ??= [];

        if ( isSuccess && errors.Count > 0 )
        {
            throw new InvalidOperationException();
        }

        if ( !isSuccess &&
             (errors.Count == 0 || errors.Any( e => e == Error.None )) )
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;

        if ( errors.Count != 0 )
        {
            Errors.AddRange( errors.Where( e => e != Error.None ) );
        }
    }

    /// <summary>
    /// Gets the list of errors associated with this response.
    /// </summary>
    public List<Error> Errors { get; } = new();

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Creates a failed response from a single error.
    /// </summary>
    /// <param name="error">The failure error.</param>
    /// <returns>A failed <see cref="LayerResponse"/> instance.</returns>
    public static LayerResponse Failure( Error error ) => new(false, error);

    /// <summary>
    /// Creates a failed response from multiple errors.
    /// </summary>
    /// <param name="errors">The failure errors.</param>
    /// <returns>A failed <see cref="LayerResponse"/> instance.</returns>
    public static LayerResponse Failure( List<Error> errors ) => new(false, errors);

    /// <summary>
    /// Converts an <see cref="Error"/> into a failed <see cref="LayerResponse"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator LayerResponse( Error error ) => Failure( error );

    /// <summary>
    /// Converts a list of <see cref="Error"/> values into a failed <see cref="LayerResponse"/>.
    /// </summary>
    /// <param name="errors">The errors to convert.</param>
    public static implicit operator LayerResponse( List<Error> errors ) => Failure( errors );

    /// <summary>
    /// Extracts the first error from a response.
    /// </summary>
    /// <param name="response">The source response.</param>
    /// <returns>The first error, or <see cref="Error.None"/> when no errors are available.</returns>
    public static implicit operator Error( LayerResponse response ) => response.Errors.FirstOrDefault() ?? Error.None;

    /// <summary>
    /// Extracts the full error list from a response.
    /// </summary>
    /// <param name="response">The source response.</param>
    /// <returns>The response errors.</returns>
    public static implicit operator List<Error>( LayerResponse response ) => response.Errors;

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    /// <returns>A successful <see cref="LayerResponse"/> instance.</returns>
    public static LayerResponse Success() => new(true, Error.None);

    /// <summary>
    /// Converts a non-generic response into a generic response.
    /// </summary>
    /// <typeparam name="TResult">The target result type.</typeparam>
    /// <param name="result">The response to convert.</param>
    /// <returns>A generic response that preserves success/failure state.</returns>
    public static LayerResponse<TResult> ToLayerResponse< TResult >( LayerResponse result ) =>
        result.IsSuccess ? LayerResponse<TResult>.Success() : LayerResponse<TResult>.Failure( result.Errors );
}
