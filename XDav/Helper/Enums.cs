using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Helper
{
    public enum StatusCode : int
    {
        	Continue	                     =100 ,      //The client can continue with its request.
        	Switching                        =101 ,      //Protocols	The server is switching protocols according to the upgrade header.
        	Processing	                     =102 ,      //The request is being processed by the server.
        	OK	                             =200 ,      //The request succeeded normally.
        	Created	                         =201 ,      //The request succeeded and created a new resource on the server.
        	Accepted	                     =202 ,      //A request was accepted for processing, but was not completed.
        	NonAuthoritativeInformation	     =203 ,      //The meta-information presented by the client did not originate from the server.
        	NoContent	                     =204 ,      //The request succeeded but that there was no new information to return.
        	Reset                            =205 ,      //Content	The agent should reset the document view that caused the request to be sent.
        	Partial                          =206 ,      //Content	The server fulfilled the partial GET request for the resource.
        	MultiStatus	                     =207 ,      //The response requires providing status for multiple independent operations.
        	MultipleChoices	                 =300 ,      //The resource has permanently moved to a new location, and that future references should use a new URL with their requests.
        	MovedPermanently	             =301 ,      //The resource has temporarily moved to another location, but that future references should still use the original URL to access the resource.
        	MovedTemporarily	             =302 ,      //The resource has temporarily moved to another location. Future references should still use the original URL to access the resource.
        	SeeOther	                     =303 ,      //A conditional GET operation found that the resource was available and not modified.
        	NotModified	                     =304 ,      //A conditional GET operation found that the resource was available and not modified.
        	UseProxy	                     =305 ,      //The requested resource must be accessed through the proxy.
        	BadRequest	                     =400 ,      //The request that the client sent was syntactically incorrect.
        	Unauthorized	                 =401 ,      //The request requires HTTP authentication.
        	PaymentRequired	                 =402 ,      //The caller must provide a payment.
        	Forbidden	                     =403 ,      //The server understood the request but refused to fulfill it.
        	NotFound	                     =404 ,      //The requested resource is not available.
        	MethodNotAllowed	             =405 ,      //The method specified is not allowed for the resource.
        	NotAcceptable	                 =406 ,      //The resource identified by the request is capable only of generating response entities that have content characteristics not acceptable according to the accept headers sent in the request.
        	ProxyAuthenticationRequired	     =407 ,      //The client must authenticate itself first with the proxy.
        	RequestTimeout	                 =408 ,      //The request could not be completed due to a conflict with the current state of the resource.
        	Conflict	                     =409 ,      //the request could not be completed due to a conflict with the current state of the resource.
        	Gone	                         =410 ,      //The resource is no longer available at the specified server location and no forwarding address is known.
        	LengthRequired	                 =411 ,      //The request cannot be handled without a defined content length.
        	PreconditionFailed	             =412 ,      //The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.
        	RequestTooLong	                 =413 ,      //The server is refusing to process a request because the request entity is larger than the server is willing or able to process.
        	UnsupportedMediaType	         =415 ,      //The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.
        	RequestedRangeNotSatisfiable	 =416 ,      //The server cannot serve the requested byte range.
        	ExpectationFailed	             =417 ,      //The server could not meet the expectation given in the Expect request header.
        	UnprocessableEntity	             =418 ,      //The entity body submitted with the PATCH method was not understood by the resource.
        	InsufficientSpaceOnResource	     =419 ,      //The resource does not have sufficient space to record its state after executing this method.
        	MethodFailure	                 =420 ,      //The method was not executed on a particular resource within its scope because some part of the method's execution failed causing the entire method to be aborted.
        	//UnprocessableEntity	             =422 ,      //The server is not able to process the contained instructions.
        	Locked	                         =423 ,      //The destination resource of a method is locked, and either the request did not contain a valid Lock-Info header, or the Lock-Info header identifies a lock held by another principal.
        	FailedDependency	             =424 ,      //The requested action depended on another action, and that action failed.
        	InternalServerError	             =500 ,      //An error inside the HTTP service prevented it from fulfilling the request.
        	NotImplemented	                 =501 ,      //The HTTP service does not support the functionality needed to fulfill the request.
        	BadGateway	                     =502 ,      //The HTTP server received an invalid response from a server it consulted when acting as a proxy or gateway.
        	ServiceUnavailable	             =503 ,      //The HTTP service is temporarily overloaded, and unable to handle the request.
        	GatewayTimeout	                 =504 ,      //The server did not receive a timely response from the upstream server while acting as a gateway or proxy.
        	HTTPVersionNotSupported	         =505 ,      //The server does not support (or refuses to support) the HTTP protocol version that was used in the request.
        	InsufficientStorage              =507,
    }
}
