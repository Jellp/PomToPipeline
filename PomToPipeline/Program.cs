using System;
using System.Text;
using System.Xml;

namespace PomToJenkinsPipeline
{
	class Program
	{
		static void Main(string[] args)
		{
			
			// Check if an argument is given, otherwise return an error
			if (args.Length == 0)
			{
				// DEBUG
				//args = new string[1];
				//args[0] = @".\testcase.xml";
				// NON-DEBUG
				Console.WriteLine("Error: File not provided");
				Console.WriteLine("USAGE: dotnet PomToJenkinsPipeline.dll [FILE]");
				Environment.Exit(1);
			}

			// Load XML
			XmlDocument pom = new XmlDocument();
			pom.Load(args[0]);

			XmlNode project = pom.GetElementsByTagName("project")[0];

			string groupId = null;
			string artifactId = null;
			string fileType = null;
			string version = null;

			// This part of the code assumes that the output file is the first groupid, artifactid etc to be found,
			// This is a bit of a hack atm, if it reaches an unconventional pom file, this will return incorrect output.

			try
			{
				groupId		= pom.GetElementsByTagName("groupId")[0].InnerText; 
				artifactId	= pom.GetElementsByTagName("artifactId")[0].InnerText; 
				fileType	= pom.GetElementsByTagName("packaging")[0].InnerText; 
				version		= pom.GetElementsByTagName("version")[0].InnerText; 
			}
			catch (Exception e)
			{
				Console.WriteLine("Unable to find a groupId, artifactId, packaging or version tag!");
				Console.WriteLine(e.ToString());
				Environment.Exit(2);
			}


			// Output
			StringBuilder output = new StringBuilder();
			output.Append('[');

			// TODO Foreach file created, this script does not support multiple file outputs at the moment.
			// This might also require some re-ordering of code, such that we can create the output whilst retrieving groupId etc.

			string element = $"[artifactId: '{artifactId}', classifier: '', file: 'target/{artifactId}-{version}.{fileType}', type: '{fileType}']";
			output.Append(element);

			output.Append(']');

			Console.Write(output.ToString());

			//Console.ReadLine();
		}
	}
}
