using System;
using System.IO;
using System.Linq;
using Roslyn.Scripting;

namespace ScriptCs.Engine.Roslyn
{
	public abstract class RoslynScriptAssemblyCompilerEngine : RoslynScriptEngine
	{
		public RoslynScriptAssemblyCompilerEngine(IScriptHostFactory scriptHostFactory)
			: base(scriptHostFactory) { }

		protected override void Execute(string code, Session session)
		{
			var submission = session.CompileSubmission<object>(code);
			var assemblyBytes = new byte[0];
			var pdbBytes = new byte[0];
			var compileSuccess = false;

			using (var exeStream = new MemoryStream())
			using (var pdbStream = new MemoryStream()) {
				var result = submission.Compilation.Emit(exeStream, pdbStream: pdbStream);
				compileSuccess = result.Success;

				if (result.Success) {
					assemblyBytes = exeStream.ToArray();
					pdbBytes = pdbStream.ToArray();
				} else {
					var errors = String.Join(Environment.NewLine, result.Diagnostics.Select(x => x.ToString()));
					Console.WriteLine(errors);
				}
			}

			if (compileSuccess) {
				Compiled(session, assemblyBytes, pdbBytes);
			}
		}

		public abstract void Compiled(Session session, byte[] assemblyBytes, byte[] pdbBytes);
	}
}
