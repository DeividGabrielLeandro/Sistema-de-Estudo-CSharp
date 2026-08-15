-- CREATE TABLE sessao_estudo (
--     id INT PRIMARY KEY IDENTITY(1,1),
--     id_cliente INT NOT NULL,
--     id_meta INT NULL,
--     titulo VARCHAR(100) NOT NULL,
--     descricao VARCHAR(1000) NULL,
--     data_inicio DATETIME NOT NULL DEFAULT GETDATE(),
--     data_fim DATETIME NULL,
--     duracao_minutos INT NULL DEFAULT 0,
--     tempo_estudado_minutos INT NULL DEFAULT 0,
--     status VARCHAR(20) NOT NULL, -- 'EM_ANDAMENTO', 'PAUSADO', 'CONCLUIDO', 'CANCELADO'

--     CONSTRAINT FK_Sessao_Estudo FOREIGN KEY (id_meta) 
--         REFERENCES Estudo(id) ON DELETE SET NULL
-- );

-- CREATE TABLE pausa_sessao (
--     id INT PRIMARY KEY IDENTITY(1,1),
--     id_sessao INT NOT NULL,
--     inicio DATETIME NOT NULL,
--     fim DATETIME NULL,
--     duracao_minutos INT NULL DEFAULT 0,
--     motivo VARCHAR(255) NULL,

--     CONSTRAINT FK_Pausa_Sessao FOREIGN KEY (id_sessao) 
--         REFERENCES sessao_estudo(id) ON DELETE CASCADE
-- );


SELECT * FROM sessao_estudo