using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearArrangement
{
    class Solucao
    {
        
        private List<VerticeMapping> linearArrangement;
        public List<VerticeMapping> LinearArrangement
        {
            get { return linearArrangement; }
            set { linearArrangement = value; }
        }

        private decimal valorSolucao;
        public decimal ValorSolucao
        {
            get { return valorSolucao; }
            set { valorSolucao = value; }
        }

        public Solucao()
        {
            LinearArrangement = new List<VerticeMapping>();           
        }
    }

    class GRASP
    {
        public int numVertices { get; set; }
        
        public int[,] Matriz { get; set; }
        public List<VerticeGrau> lstVerticeGrau { get; set; }

        //private static

        public GRASP()
        {
            this.Matriz = new int[numVertices, numVertices];
            this.lstVerticeGrau = new List<VerticeGrau>();
        }
        
        public Solucao grasp(int numIter, decimal pAlfa)
        {
            Solucao solucaoInicial = new Solucao();
            solucaoInicial.ValorSolucao = decimal.MaxValue;

            Solucao solucao = new Solucao();
            solucaoInicial.ValorSolucao = decimal.MaxValue;

            bool PrimeiraSolucao = true;

            int num_iter = 0;

            while (num_iter < numIter)
            {
                Console.WriteLine("Iteracao : {0}", num_iter);

                DateTime tempoInicial = DateTime.Now;
                solucaoInicial = greedyRandomizedSolution(pAlfa);
                DateTime tempoFinal = DateTime.Now;
                Console.WriteLine("Tempo de execução gulosidade - randomica(em segundos): {0} ", (tempoFinal.Subtract(tempoInicial)).TotalSeconds);
                Console.WriteLine();
                tempoInicial = DateTime.Now;
                solucaoInicial = buscaLocal(numIter, solucaoInicial);
                tempoFinal = DateTime.Now;
                Console.WriteLine("Tempo de execução busca local(em segundos): {0} ", (tempoFinal.Subtract(tempoInicial)).TotalSeconds);
                Console.WriteLine();

                if (PrimeiraSolucao)
                {
                    solucao.ValorSolucao = solucaoInicial.ValorSolucao;
                    solucao.LinearArrangement = solucaoInicial.LinearArrangement;
                    PrimeiraSolucao = false;
                }

                if (solucaoInicial.ValorSolucao < solucao.ValorSolucao)
                {
                    Console.WriteLine("Solução atualizada: {0}", solucaoInicial.ValorSolucao);
                    Console.WriteLine();

                    solucao.ValorSolucao = solucaoInicial.ValorSolucao;
                    solucao.LinearArrangement = solucaoInicial.LinearArrangement;                    
                }

                num_iter++;
            }

            return solucao;

        }

        private Solucao greedyRandomizedSolution(decimal alfa)
        {   
            Solucao sol = new Solucao();            
            Random rand = new Random();
             
           int verticesAdicionados = 0;
           int mapeamento = 1;
           while (verticesAdicionados < numVertices)
           {
               List<VerticeGrau> listaCandidatos = new List<VerticeGrau>();
               foreach (VerticeGrau item in lstVerticeGrau)
               {
                   listaCandidatos.Add(item);
               }

               List<VerticeGrau> LRC = criarLRC(alfa, listaCandidatos);
               int verticeRandomico = rand.Next(0, numVertices);

               if (sol.LinearArrangement.Find(x => x.Vertice == verticeRandomico) == null)
               {
                   VerticeMapping arrangement = new VerticeMapping();
                   arrangement.Vertice = verticeRandomico;
                   arrangement.Mapeamento = mapeamento;
                   sol.LinearArrangement.Add(arrangement);

                   mapeamento++;
                   verticesAdicionados++;
               }

           }
           
            CalcularValorSolucao(sol);
            
            return sol;

        }

        private Solucao buscaLocal(int numeroIter, Solucao sol)
        {

            Solucao novaSolucao = new Solucao();
            novaSolucao.ValorSolucao = sol.ValorSolucao; ;
            foreach (VerticeMapping item in sol.LinearArrangement)
            {
                novaSolucao.LinearArrangement.Add(item);
            }

            Random rand = new Random(DateTime.Now.Minute);

            int no_update = 0;

            decimal solucaoAntiga = sol.ValorSolucao;
            bool encontrou = false;

            for (int linha = 0; linha < numVertices; linha++)
            {
                if (encontrou)
                    break;
                for (int coluna = linha + 1; coluna < numVertices; coluna++)
                {
                    if (linha < coluna) // percorre os vertices incidentes somente na matriz triangular superior
                    {
                        if (no_update < numeroIter)
                        {
                            if (Matriz[linha, coluna] == 1)
                            {
                                int mapeamentoTemp = novaSolucao.LinearArrangement[coluna].Mapeamento;
                                novaSolucao.LinearArrangement[coluna].Mapeamento = novaSolucao.LinearArrangement[linha].Mapeamento;
                                novaSolucao.LinearArrangement[linha].Mapeamento = mapeamentoTemp;

                                CalcularValorSolucao(novaSolucao);

                                if (novaSolucao.ValorSolucao < sol.ValorSolucao)
                                {
                                    sol.ValorSolucao = novaSolucao.ValorSolucao;
                                    sol.LinearArrangement = novaSolucao.LinearArrangement;
                                    encontrou = true;
                                    break;

                                    //solucaoAntiga = sol.ValorSolucao;
                                    //sol.ValorSolucao = solucaoAntiga;
                                    //encontrou = true;
                                    //break;
                                }
                                else
                                {
                                    no_update++;
                                }
                            }
                        }
                        else
                        {
                            encontrou = true;
                            break;
                        }
                    }
                }
            }
            /*
            for (int i = 0; i < novaSolucao.LinearArrangement.Count; i++)
            {
                if (encontrou)
                    break;
                for (int j = i + 1; j < novaSolucao.LinearArrangement.Count; j++)
                {
                    if (no_update < numeroIter)
                    {
                        //VerticeMapping temp = novaSolucao.LinearArrangement[j];
                        //novaSolucao.LinearArrangement[j] = novaSolucao.LinearArrangement[i];
                        //novaSolucao.LinearArrangement[i] = temp;
                        int mapeamentoTemp = novaSolucao.LinearArrangement[j].Mapeamento;
                        novaSolucao.LinearArrangement[j].Mapeamento = novaSolucao.LinearArrangement[i].Mapeamento;
                        novaSolucao.LinearArrangement[i].Mapeamento = mapeamentoTemp;

                        CalcularValorSolucao(novaSolucao);

                        if (novaSolucao.ValorSolucao < sol.ValorSolucao)
                        {
                            sol.ValorSolucao = novaSolucao.ValorSolucao;
                            sol.LinearArrangement = novaSolucao.LinearArrangement;
                            encontrou = true;
                            break;

                            //solucaoAntiga = sol.ValorSolucao;
                            //sol.ValorSolucao = solucaoAntiga;
                            //encontrou = true;
                            //break;
                        }
                        else
                        {
                            no_update++;
                        }
                    }
                    else
                    {
                        encontrou = true;
                        break;
                    }
                                       
                }
            }
             */

            return sol;

        }

        private static List<VerticeGrau> criarLRC(decimal alfa, List<VerticeGrau> listaCandidatos)
        {
            List<VerticeGrau> listaRestritaCandidatos = new List<VerticeGrau>();
            List<VerticeGrau> listaOrdenada = (from p in listaCandidatos orderby p.grauVertice select p).ToList();

            listaOrdenada.ForEach(delegate(VerticeGrau verticeGrau)
            {
                if (listaRestritaCandidatos.Count < ((listaOrdenada.Count * alfa) + 1))
                {
                    listaRestritaCandidatos.Add(verticeGrau);
                }

            });

           return listaRestritaCandidatos;
           
        }

        private void CalcularValorSolucao(Solucao sol)
        {
            for (int linha = 0; linha < numVertices; linha++)
            {
                for (int coluna = 0; coluna < numVertices; coluna++)
                {
                    if (linha < coluna) // percorre os vertices incidentes somente na matriz triangular superior
                    {
                        if (Matriz[linha, coluna] == 1)
                        {
                            int mapVerticeU = sol.LinearArrangement[linha].Mapeamento;
                            int mapVerticeV = sol.LinearArrangement[coluna].Mapeamento;

                            //int mapVerticeU = sol.LinearArrangement.IndexOf(linha) + 1;
                            //int mapVerticeV = sol.LinearArrangement.IndexOf(coluna) + 1;

                            decimal distanciaVertices = Math.Abs(mapVerticeU - mapVerticeV);
                            sol.ValorSolucao += distanciaVertices;                            
                        }                        
                    }                        
                }
            }  
        }
    }
}
