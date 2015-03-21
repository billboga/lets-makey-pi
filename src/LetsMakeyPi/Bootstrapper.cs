using Nancy;
using Nancy.Conventions;

namespace LetsMakeyPi
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions
                .StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("Scripts"));
        }
    }
}
