using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //using var db = new Data.ApplicationContext();
            
            //db.Database.Migrate();
            //var existe = db.Database.GetPendingMigrations().Any();
            
            //if (existe)
            //{
                //
            //}

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultaPedidoCarregamentoAdiantado();
            //AtualizaDados();
            RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(2);
            var cliente = new Cliente { Id = 3 };
            
            //db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;
            
            db.SaveChanges();
        }

        private static void AtualizaDados()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(1);
            var cliente = new Cliente
            {
                Id = 1
            };

            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado Passo 3",
                Telefone = "31831213"
            };

            //cliente.Nome = "Cliente Alterado Passo 2";

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            //db.Entry(cliente).State = EntityState.Modified;
            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedidos
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };   

            db.Pedidos.Add(pedido);
            db.SaveChanges();         
        }

        private static void ConsultaPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedido = db
                .Pedidos
                .Include(p=>p.Itens)
                    .ThenInclude(p=>p.Produto)
                .ToList();

            Console.WriteLine(pedido.Count);
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consulraPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p=>p.Id > 0)
                .OrderBy(p=>p.Id)
                .ToList();
            
            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente:  {cliente.Id}");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p=>p.Id==cliente.Id);
            }
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            //db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "123456789121",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Marco Servio",
                CEP = "1234567",
                Cidade = "Contagem",
                Estado = "MG",
                Telefone = "123456789"
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Paulo Servio",
                    CEP = "1234567",
                    Cidade = "Contagem",
                    Estado = "MG",
                    Telefone = "123456789"
                },
                new Cliente
                {
                    Nome = "Lucas Servio",
                    CEP = "1234567",
                    Cidade = "Contagem",
                    Estado = "MG",
                    Telefone = "123456789"
                },
            };

            using var db = new Data.ApplicationContext();
            //db.AddRange(produto, cliente);
            db.Set<Cliente>().AddRange(listaClientes);
            //db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total Registro(s): {registros}");
        }
    }
}
