using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;

namespace AkkaUniverse
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var system = ActorSystem.Create("AkkaUniverse"))
			{
				ActorRef helloWorldActorRef = system.ActorOf(Props.Create<HelloWorldActor>());
				
				Console.WriteLine("Press any key to say hello to the world...");
				while (true)
				{	
					Console.ReadLine();
					helloWorldActorRef.Tell(new SayHelloWorldMessage(shouldSayHelloToUniverse: true, shouldWaitForAnswer: true));
				}
			}
		}
	}

	public class HelloWorldActor : TypedActor, IHandle<SayHelloWorldMessage>
	{
		private ActorRef HelloUniverseActorRef;

		public HelloWorldActor()
		{
			HelloUniverseActorRef = Context.ActorOf(Props.Create<HelloUniverseActor>());	
		}

		public void Handle(SayHelloWorldMessage message)
		{
			if (message.ShouldSayHelloToUniverse)
			{
				HelloUniverseActorRef.Tell(message);
			}

			Console.WriteLine("Hello World!");
		}
	}

	public class HelloUniverseActor : TypedActor, IHandle<SayHelloWorldMessage>
	{
		public void Handle(SayHelloWorldMessage message)
		{
			Console.WriteLine("Hello Universe?");

			if (message.ShouldWaitForAnswer)
			{
				Thread.Sleep(5000);//fake wait
				Console.WriteLine("Meh! I give up waiting for an answer.");
			}
		}
	}

	public class SayHelloWorldMessage
	{
		public SayHelloWorldMessage(bool shouldSayHelloToUniverse, bool shouldWaitForAnswer)
		{
			ShouldWaitForAnswer = shouldWaitForAnswer;
			ShouldSayHelloToUniverse = shouldSayHelloToUniverse;
		}

		public bool ShouldSayHelloToUniverse { get; private set; }
		public bool ShouldWaitForAnswer { get; private set; }
	}
}
