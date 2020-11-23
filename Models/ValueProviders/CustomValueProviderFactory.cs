using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAppExample.Models.ValueProviders
{
    public class CustomValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new System.NotImplementedException();
            }

            context.ValueProviders.Add(new CustomValueProvider(BindingSource.ModelBinding));

            return Task.CompletedTask;
        }
    }
}