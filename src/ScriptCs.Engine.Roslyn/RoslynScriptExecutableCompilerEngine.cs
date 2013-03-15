using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Scripting;

namespace ScriptCs.Engine.Roslyn
{
	public class RoslynScriptExecutableCompilerEngine : RoslynScriptAssemblyCompilerEngine
	{
		public RoslynScriptExecutableCompilerEngine(IScriptHostFactory scriptHostFactory)
			: base(scriptHostFactory) { }

		public override void Compiled(Session session, byte[] assemblyBytes, byte[] pdbBytes)
		{
			// make executable here
		}
	}
}
