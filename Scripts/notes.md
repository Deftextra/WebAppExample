* In the MVC pattern the model is technically the Data.
* The view is the page as seen by the client. Now each view depends on a model.
* This model is created by the Controller which the re
* Asp.net core is build on top of ASP.NET core routing.

# ASP.NET Controller routing.

* Actions on controllers can be either conventionally-routed or attribute-routed.
* Conventional routing is used with controllers and views - The mapping is based on
  the controller and action names only. It is not based on namespaces, source file locations,
    or method parameters.
* MaiControllerRoute and MapAreaRoute use order value for the endpoints.
* All action Endpoints are created before the application starts!!
* A routing order is specified for each endpoint
* Routing is the proccesss of attaching models to endpoints

# Review Model binding.

* Remember that query strings are not part of the resource location so Endpoint matching will ignore this.
* All validations are stored in the modelState property,
* Modelling --> model validation.
* route --> Query String --> 

* Model binding tries to find target to parameters of the method first.
* Model binding targets the following things on a class
 1. parameters of the action.
2. public properties of a page model controller. 

* model binding targets for properties and parameters occurs in the following order.

1. Form fields (from body)
2. The request body (This is only true for API controllers)
3. Route data
4. Query string.
5. Upload data.

* Route data and query string are only used for simple types. 
* Uploadabl files are bound only to target types that implement IFormFile or IEnumerable<IFormfile>.
* We the model binder does the follwing. ones the endpoint action has been found, Model binding is done.
* In model binding we have targets and sources.
* Targets can be parameters and public properties.

## Model binding

* Value providers are the components that feed data to model binders.
### Custom model binders 
* Base64 encoding converts bytes into character encoding (using ASCII).

* The contains prefix method is called by the model binder to determing wheter the value provider has th data for a given prefix.
* The value provider decides
* Before the valueProider is created, the value provider factory pass in the keyvalue data which has already been parsed by the aps.net engine.


#### Overview.
* First a controller is found.
* During a http request a spefic action on a controller is found.
* Then all the value providers are set giving a key value pair dictionary for each request context.
* The key always corrosponds to the parameter name of an action or a public property of the controller.
* The paramterName has a type (model) which can be either complex or simple.
* The model binder is searched for the particular type of model. This is done in the order the
  model binders are registed in the provider. The first model binder is chosen.
 * The model binder then uses the parameter names as a key in the value provider to find the 
   corresponding data to bind the model with. In this case, a string always crossponds to a single model.
 * What we use as key all depends on the model binder!!!!


# DI container TODO
* Go over DI container implementation in asp.net core
 
 
 
