using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearArrangement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Linha de comando correta é: LinearArrangement [nome-do-arquivo] [numero-maximo-iteracoes] [alfa]");
                }                
                else if (args.Length == 3)
                {
                    string nomeArquivo = string.Empty;
                    if (!String.IsNullOrEmpty(args[0]))
                    {
                        nomeArquivo = args[0];
                    }

                    int numeroIteracoes = Convert.ToInt32(args[1]);
                    decimal alfa = Convert.ToDecimal(args[2], new CultureInfo("en-US"));

                    Parser parser = new Parser(nomeArquivo);
                    parser.InputLinearArrangementFile();
                   

                    //StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + nomeArquivo + ".dat");
                    //for (int i = 0; i < parser.ListaVerticesGrau.Count; i++)
                    //{
                    //    for (int j = i; j < parser.ListaVerticesGrau.Count; j++)
                    //    {
                    //        if (i < j)
                    //        {
                    //            if (parser.matriz[i, j] == 1)
                    //            {
                    //                file.Write("{0} {1},", i+1, j+1);
                    //            }
                    //        }
                    //    }                     
                    //}

                    //file.Flush();
                    
                    GRASP grasp = new GRASP();
                    grasp.Matriz = parser.matriz;
                    grasp.numVertices = parser.ListaVerticesGrau.Count;
                    grasp.lstVerticeGrau = parser.ListaVerticesGrau;
                    DateTime tempoInicial = DateTime.Now;
                    Solucao solucao = grasp.grasp(numeroIteracoes, alfa);
                    DateTime tempoFinal = DateTime.Now;

                    Console.WriteLine("Instancia: {0}", nomeArquivo);
                    Console.WriteLine("Sequencia de labels");
                    for (int i = 0; i < solucao.LinearArrangement.Count; i++)
                    {
                        Console.Write("Vertice {0} com mapeamento para {1} ", solucao.LinearArrangement[i].Vertice + 1, solucao.LinearArrangement[i].Mapeamento);
                        Console.WriteLine();
                    }
                    Console.WriteLine("Solução encontrada = {0}", solucao.ValorSolucao);
                    Console.WriteLine("Alfa: {0} ", alfa);
                    Console.WriteLine("Numero interacoes: {0} ", numeroIteracoes);
                    Console.WriteLine("Tempo de execução(em segundos): {0} ", (tempoFinal.Subtract(tempoInicial)).TotalSeconds);
                    
                                        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
