using Godot;
using Jint;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class JintMachine : Node
{
	bool running = false;
	Jint.Engine machine;
	string context = "{\"test\":\"1\"}";
	double dx;
	double dy;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		machine = new Jint.Engine(options =>
		{
			// Limit memory allocations to MB
			//options.LimitMemory(4_000_000);

			// Set a timeout to 4 seconds.
			options.TimeoutInterval(TimeSpan.FromSeconds(4));

			// Set limit of 1000 executed statements.
			//options.MaxStatements(1000);

			// Use a cancellation token.
			//options.CancellationToken(cancellationToken);
		});
		machine.SetValue("log", new Action<string>((x)=>GD.Print(x)));
		machine.SetValue("dx", 0);
		machine.SetValue("dy", 0);
		try{
		machine.Execute(
			@"
			let context = {}
			const parseContext = () => {
				context = JSON.parse(jsonContext)
			}
			const process = async (ctx) => {
				//log(JSON.stringify(ctx))
				dx = dx + 1
				dy = dy + 1.1
				log('OK')
				const myPromise = await new Promise( (r) => { log('P'); r() } )
				log('KO')
			}
			"
		);}catch(Exception e){GD.Print(e);}
	}
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	  AsyncFunction(nameof(_Process));
  }

private async Task AsyncFunction(string from)
	{
		if(running){return;}
		running = true;
		await Task.Delay(1000);
		double startTime = OS.GetTicksUsec();

		machine.SetValue("jsonContext",context);
		try{
		machine.Execute(
			@"
			parseContext();
			(async ()=>{process(context)})(); 
			"
		);}catch(Exception e){GD.Print(e);}
		double elapsed = OS.GetTicksUsec()-startTime;

		dx = machine.GetValue("dx").AsNumber();
		dy = machine.GetValue("dy").AsNumber();
		GD.Print(elapsed);
		GD.Print(dx);
		GD.Print(dy);
		
		//machine.GetValue
		running = false;
		
	}
	 
}
