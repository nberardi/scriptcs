using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Roslyn.Scripting;
using ScriptCs.Exceptions;

namespace ScriptCs.Engine.Roslyn
{
	public class RoslynScriptDebuggerEngine : RoslynScriptAssemblyCompilerEngine
    {
        private const string CompiledScriptClass = "Submission#0";
        private const string CompiledScriptMethod = "<Factory>";

        public RoslynScriptDebuggerEngine(IScriptHostFactory scriptHostFactory)
            : base(scriptHostFactory) { }

		public override void Compiled(Session session, byte[] assemblyBytes, byte[] pdbBytes)
		{
			var assembly = AppDomain.CurrentDomain.Load(assemblyBytes, pdbBytes);
			var type = assembly.GetType(CompiledScriptClass);
			var method = type.GetMethod(CompiledScriptMethod, BindingFlags.Static | BindingFlags.Public);

			try {
				method.Invoke(null, new[] { session });
			} catch (Exception e) {
				var message =
					string.Format(
					"Exception Message: {0} {1}Stack Trace:{2}",
					e.InnerException.Message,
					Environment.NewLine,
					e.InnerException.StackTrace);
				throw new ScriptExecutionException(message);
			}
        }
    }
}