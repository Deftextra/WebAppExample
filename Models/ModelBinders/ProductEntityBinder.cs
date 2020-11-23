using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using IModelBinder = Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder;
using ModelBindingContext = Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext;
using ValueProviderResult = Microsoft.AspNetCore.Mvc.ModelBinding.ValueProviderResult;

namespace WebAppExample.Models.ModelBinders
{
    public class ProductEntityBinder : IModelBinder
    {
        private readonly DataContext _dbContext;

        public ProductEntityBinder(DataContext context)
        {
            _dbContext = context;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;

            // modelName is the prefix name.

            var valueProvider = bindingContext.ValueProvider;

            if (valueProvider == null)
            {
                throw new NullReferenceException("Value provider must be set");
            }
            
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

           if (!long.TryParse(value, out var id))
            {
                bindingContext.ModelState.TryAddModelError(
                    modelName, "Product Id must be an integer");
                return Task.CompletedTask;
            }

            // in this case the model is the entire thing we are trying to created.
            var model = _dbContext.Products.Find(id);
            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }
}