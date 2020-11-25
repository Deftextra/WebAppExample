* The follwowing routing parameters are reserved when using controllers or Razor pages.
1. Action
2. area
3. controller
4. handler
5. page

* The [ApiController] make the followking properties of a controller neccesary
1. Controller must be attribute routed.
2. ApiController attribute makes model validation erros trigger automatically tirgger a HTTP 400 ruest by default.
3. By default it does the follwing
    * [FromBody] is inferred for complex type parameters. Expception is any build in complex type with special meaning
    * [FromForm] is inferred for action parameters of tyep IFormFile and IformFileCollection.
    * [FromRoute] is inferred for any actin parameter name matching a parameter in the route template.
    * by default the body must of json type for API controller attribute,jk
    * By default prameters are required
    * Route value providers are mutually exclusive,

* Adding a route attribute to the controller make every action a route attribute.
* Attribute routing does not use the ordering property of endpoints
* We need to avoid using the order popery on routing attributes
* By default a model state error is not raised when no value is founded for a model  property.
* The propety is set to null or default.
    1. Nullable simple types are set to null.
    2. Non nullable value types are set to default.
    3. For complex types, model bindings creates an instance by using the deafult.
       Constructor. Without setting properties.





## REST

* Get request should be read-only inforamation retrieval and should be save. That is,
it does not change anything on the server. (It is a impodent operation,)

