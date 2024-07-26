CREATE TABLE [dbo].[Clienti] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [codice_fiscale] VARCHAR(16) UNIQUE NOT NULL,
    [cognome] VARCHAR(50) NOT NULL,
    [nome] VARCHAR(50) NOT NULL,
    [città] VARCHAR(50) NOT NULL,
    [provincia] VARCHAR(50) NOT NULL,
    [email] VARCHAR(100) NOT NULL,
    [cellulare] VARCHAR(15) NOT NULL,
    CONSTRAINT [chk_codice_fiscale_length] CHECK (LEN([codice_fiscale]) = 16)
);

CREATE TABLE [dbo].[Camere] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Descrizione] NVARCHAR(255) NOT NULL,
    [Tipologia] NVARCHAR(255) NOT NULL,
    [Prezzo] DECIMAL(10, 2) NOT NULL
);


CREATE TABLE [dbo].[Users] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [username] VARCHAR(50) UNIQUE NOT NULL,
    [password] VARCHAR(50) NOT NULL,
    [role] VARCHAR(50) NOT NULL  
);


CREATE TABLE [dbo].[Prenotazioni] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [cliente_id] INT NOT NULL,
    [camera_id] INT NOT NULL,
    [data_prenotazione] DATE NOT NULL,
    [numero_progressivo] INT NOT NULL,
    [anno] INT NOT NULL,
    [dal] DATE NOT NULL,
    [al] DATE NOT NULL,
    [caparra] DECIMAL(10, 2) NOT NULL,
    [tariffa] DECIMAL(10, 2) NOT NULL,
    [tipologia_soggiorno] VARCHAR(255) NOT NULL,
    FOREIGN KEY ([cliente_id]) REFERENCES [dbo].[Clienti]([id]),
    FOREIGN KEY ([camera_id]) REFERENCES [dbo].[Camere]([id]),
    CONSTRAINT chk_tipologia_soggiorno CHECK (tipologia_soggiorno IN ('mezza pensione', 'pensione completa', 'pernottamento con prima colazione'))
);--per la tipologia soggiorno,essendo valori fissi , non implementabili o cancellabili, non ho voluto fare una tabella, ma metterli così 


CREATE TABLE [dbo].[Servizi] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [descrizione] VARCHAR(100) NOT NULL,
    [prezzo] DECIMAL(10, 2) NOT NULL
);

CREATE TABLE [dbo].[Prenotazioni_Servizi] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [prenotazione_id] INT NOT NULL,
    [servizio_id] INT NOT NULL,
    FOREIGN KEY ([prenotazione_id]) REFERENCES [dbo].[Prenotazioni]([id]),
    FOREIGN KEY ([servizio_id]) REFERENCES [dbo].[Servizi]([id])
);





-- Inserimento dati nella tabella Clienti
INSERT INTO [dbo].[Clienti] ([codice_fiscale], [cognome], [nome], [città], [provincia], [email], [cellulare])
VALUES 
('RSSMRA80A01H501Z', 'Rossi', 'Mario', 'Milano', 'MI', 'mario.rossi@example.com', '3291234567'),
('VRDLGI85C09F205Z', 'Verdi', 'Luigi', 'Roma', 'RM', 'luigi.verdi@example.com', '3391234567');

-- Inserimento dati nella tabella Camere(ho aggiunto il prezzo di ogni stanza , mi sembrava sensato aggiungerlo qui) 
INSERT INTO [dbo].[Camere] ([Descrizione], [Tipologia], [Prezzo])
VALUES 
('Camera singola con vista mare', 'singola', 75.00),
('Camera doppia con balcone', 'doppia', 120.00),
('Suite presidenziale', 'suite', 300.00);


-- Inserimento dati nella tabella Users
INSERT INTO [dbo].[Users] ([username], [password], [role])
VALUES 
('admin', 'adminpass', 'admin'),
('receptionist', 'receptionpass', 'receptionist');

-- Inserimento dati nella tabella Prenotazioni
INSERT INTO [dbo].[Prenotazioni] 
    ([cliente_id], [camera_id], [data_prenotazione], [numero_progressivo], [anno], [dal], [al], [caparra], [tariffa], [tipologia_soggiorno])
VALUES 
    (1, 1, '2024-07-01', 1, 2024, '2024-07-10', '2024-07-20', 100.00, 500.00, 'mezza pensione'),
    (2, 2, '2024-07-05', 2, 2024, '2024-07-15', '2024-07-25', 200.00, 1000.00, 'pensione completa');


-- Inserimento dati nella tabella Servizi
INSERT INTO [dbo].[Servizi] ([descrizione], [prezzo])
VALUES 
('Colazione in camera', 15.00),
('Bevande e cibo nel mini bar', 10.00),
('Internet', 5.00);

-- Inserimento dati nella tabella Prenotazioni_Servizi
INSERT INTO [dbo].[Prenotazioni_Servizi] 
    ([prenotazione_id], [servizio_id])
VALUES 
    (1, 1),
    (1, 2),
    (2, 2),
    (2, 3);

    SELECT * FROM SERVIZI;
    SELECT * FROM CLIENTI;
    SELECT * FROM Camere;
    USE GestionaleHotelDefinitivo;
    SELECT * FROM USERS;

 


