using System;
using ConnectorRest_V2;
using Topshelf;

namespace ContainerBiz
{
	class Program
	{
		static void Main(string[] args)
		{
            var serviceName = IniConfig.Read("config", "ServicName", "*", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ContainerBiz.ini"));
            var serviceId = IniConfig.Read("config", "ServiceId", "*", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ContainerBiz.ini"));

			string description = string.Format("ContainerBiz - {0} - {1}",
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                serviceName);


			Host host = HostFactory.New(x =>
			{
				x.Service<ContainerBizSrv>(s =>
				{

					s.ConstructUsing(sc => new ContainerBizSrv());
					s.WhenStarted(tc => tc.Start());
					s.WhenStopped(tc => tc.Stop());
				});

				x.SetServiceName(serviceName);
				x.SetDisplayName(description);
				x.SetDescription(description);

				x.StartAutomatically();
				x.RunAsNetworkService();
			});

			host.Run();

		}
	}
}
