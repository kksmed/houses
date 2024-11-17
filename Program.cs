using System.Diagnostics;

Solver solver = new();

var sw = Stopwatch.StartNew();
var (mostHouses, bestSolution) = solver.Solve();
sw.Stop();

bestSolution.Print();

Console.WriteLine();
Console.WriteLine($"With: {mostHouses} houses");

Console.WriteLine();
Console.WriteLine($"In: {sw.Elapsed}");
Console.WriteLine();
Console.WriteLine("<Press any key to exit>");
Console.ReadKey();
