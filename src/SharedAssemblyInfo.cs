using System.Reflection;

[assembly: AssemblyProduct("Zebus Tiny Host")]
[assembly: AssemblyDescription("A lightweight Zebus Host")]
[assembly: AssemblyCompany("ABC Arbitrage Asset Management")]
[assembly: AssemblyCopyright("Copyright © ABC Arbitrage Asset Management 2015")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif