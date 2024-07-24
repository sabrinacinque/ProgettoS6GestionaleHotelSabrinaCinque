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
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [descrizione] TEXT NOT NULL,
    [tipologia] VARCHAR(20) NOT NULL
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
    [tipologia_soggiorno] VARCHAR(50) NOT NULL,
    FOREIGN KEY ([cliente_id]) REFERENCES [dbo].[Clienti]([id]),
    FOREIGN KEY ([camera_id]) REFERENCES [dbo].[Camere]([id])
);


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

-- Inserimento dati nella tabella Camere
INSERT INTO [dbo].[Camere] ([descrizione], [tipologia])
VALUES 
('Camera singola con vista mare', 'singola'),
('Camera doppia con balcone', 'doppia');

-- Inserimento dati nella tabella Users
INSERT INTO [dbo].[Users] ([username], [password], [role])
VALUES 
('admin', 'adminpass', 'admin'),
('receptionist', 'receptionpass', 'receptionist'),
('manager', 'managerpass', 'manager');

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
('Bevande e cibo nel mini bar', 10.00);
('Internet', 5.00);

-- Inserimento dati nella tabella Prenotazioni_Servizi
INSERT INTO [dbo].[Prenotazioni_Servizi] 
    ([prenotazione_id], [servizio_id])
VALUES 
    (1, 1),
    (1, 2),
    (2, 2),
    (2, 3);


--query da salvare
-- Dettaglio della prenotazione al checkout
SELECT 
    c.descrizione AS numero_camera,
    p.dal,
    p.al,
    p.tariffa
FROM 
    [dbo].[Prenotazioni] p
JOIN 
    [dbo].[Camere] c ON p.camera_id = c.id
WHERE 
    p.id = 1; -- qu mettiamo poi l'id della prenotazione che vogliamo 


--2. Query per ottenere la lista di tutti i servizi aggiuntivi richiesti durante il soggiorno

SELECT 
    s.descrizione,
    ps.quantità,
    ps.prezzo,
    ps.data
FROM 
    [dbo].[Prenotazioni_Servizi] ps
JOIN 
    [dbo].[Servizi] s ON ps.servizio_id = s.id
WHERE 
    ps.prenotazione_id = 2; -- Sostituire con l'ID della prenotazione specifica


--3. Query per ottenere l'importo da saldare (tariffa – caparra + somma di tutti i servizi aggiuntivi)

SELECT 
    p.tariffa - p.caparra + ISNULL(SUM(ps.quantità * ps.prezzo), 0) AS importo_da_saldare
FROM 
    [dbo].[Prenotazioni] p
LEFT JOIN 
    [dbo].[Prenotazioni_Servizi] ps ON p.id = ps.prenotazione_id
WHERE 
    p.id = 1 -- Sostituire con l'ID della prenotazione specifica
GROUP BY 
    p.tariffa, p.caparra;


-- Ricerca prenotazioni per codice fiscale
SELECT *
FROM 
    [dbo].[Prenotazioni] p
JOIN 
    [dbo].[Clienti] cl ON p.cliente_id = cl.id
JOIN 
    [dbo].[Camere] c ON p.camera_id = c.id
WHERE 
    cl.codice_fiscale = @codice_fiscale; -- da sostituire poi con il codice fiscale in questione


    -- Numero totale di prenotazioni per soggiorni di tipo “pensione completa”
SELECT 
    COUNT(*) AS tot_pren_con_pensione_completa
FROM 
    [dbo].[Prenotazioni]
WHERE 
    tipologia_soggiorno = 'pensione completa';

SELECT * FROM Clienti;

USE GestionaleHotel;