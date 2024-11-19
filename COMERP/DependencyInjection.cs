using FluentValidation.AspNetCore;



namespace COMERP
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection srv)
        {




            // Register FluentValidation validators from the same assembly
            //srv.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateEmployeeValidator>());


            return srv;
        }
    }
}
