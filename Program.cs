using Init_db;
using Microsoft.Data.SqlClient;

Cliente cliente = new Cliente();

int id = cliente.CadastrarCliente();

Estudo.CadastrarMeta(id);