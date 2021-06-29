using FlatoutCMS.ConntentManagement.Content;
using FlatoutCMS.ConntentManagement.Context;
using FlatoutCMS.ContentParsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlatoutCMS.ConntentManagement
{
    public static class InversionOfControlHelper
    {
        public static void AddContentManagement(this IServiceCollection services, string rootPath, IConfiguration configuration, Assembly assembly)
        {
            services.AddSingleton(new DirectorySearcher($@"{rootPath}\Data\{configuration["Folders:Published"]}\"));
            services.AddSingleton(new ViewModelFinder($@"{rootPath}\Views\{configuration["Folders:Views"]}\"));
            services.AddSingleton<DataFileFetcher>();
            services.AddSingleton<IYamlParser>(new YamlParser());
            services.AddSingleton<IMarkdownParser>(new MarkdownParser());
            services.AddSingleton<DataParserFactory>();
            services.AddSingleton<IAssemblyEmbryo>(new AssemblyEmbryo(assembly));
            services.AddSingleton<ModelTypeFetcher>();
            services.AddSingleton<ModelRepository>();
            services.AddSingleton<PageRepository>();
        }
    }
}
