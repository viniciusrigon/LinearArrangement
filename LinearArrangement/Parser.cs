using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearArrangement
{
    class Parser
    {
        FileManager fm;
        public int[,] matriz;
        public List<VerticeGrau> ListaVerticesGrau { get; set; }
        
        public Parser(string nomeArquivo)
        {
            fm = new FileManager(nomeArquivo);
            ListaVerticesGrau = new List<VerticeGrau>();
        }

        public void InputLinearArrangementFile()
        {
            List<string> linha = new List<string>();

            
            int numVertices = 0;
            int numArestas = 0;

            linha = fm.ReadFileLine(); // primeira linha ( número de vértices ) 
            numVertices = Convert.ToInt32(linha[0]);

            linha = fm.ReadFileLine(); // segunda linha ( número de arcos )
            numArestas = Convert.ToInt32(linha[0]);
            matriz = new int[numVertices, numVertices];
            List<string> grausVertices = fm.ReadFileLine(); // terceira linha ( graus dos vértices )

            List<string> vizinhosVertices = fm.ReadFileLine(); // quarta linha ( vizinhos do vértice i )

            List<string> somaAcumuladoGraus = fm.ReadFileLine(); // quinta linha ( soma acumulada dos graus até o vértice i )

            int contadorArestas = 0;
            int vertice = 0;
            foreach (string grau in grausVertices)
            {
                VerticeGrau verticeGrau = new VerticeGrau();

                int qtdeVizinhos = Convert.ToInt32(grau);

                verticeGrau.numVertice = vertice;
                verticeGrau.grauVertice = qtdeVizinhos;
                ListaVerticesGrau.Add(verticeGrau);


                List<string> vizinhos = vizinhosVertices.GetRange(contadorArestas, qtdeVizinhos);
                contadorArestas += qtdeVizinhos;
                foreach (string aresta in vizinhos)
                {                    
                    matriz[Convert.ToInt32(aresta), vertice] = 1;
                }

                vertice++;                

            }

            //for (int i = 0; i < numVertices; i++)
            //{
            //    for (int j = 0; j < numVertices; j++)
            //    {
            //        Console.Write(matriz[i, j]);
            //    }
            //    Console.WriteLine();
            //}


            //for (int i = 0; i < numVertices; i++)
            //{
            //    for (int j = i; j < numVertices; j++)
            //    {
            //        if (i < j)
            //            Console.WriteLine(matriz[i, j]);
            //    }
            //}
        }
    }
}
