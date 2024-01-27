using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Personnel.Domain.Core.Bus;
using Personnel.Infra.Bus;

namespace Personnel.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain InMemoryBus MediatR
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            //Domain Handlers
            //services.AddScoped<IRequestHandler<CreateCourseCommand, bool>, CourseCommandHandler>();

        }
    }
}
