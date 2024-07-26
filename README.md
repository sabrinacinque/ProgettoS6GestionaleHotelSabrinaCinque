****PER IL PROF****
Inizialmente, se non si fa il login , è visibile solo la homepage con i link e la pagina di login( se si clicca su qualche link, automaticamente si viene reindirizzati al login).
Ho previsto due login diversi per implementare la questione delle policy e delle autorizazioni:l'admin ( inteso ccome amministratore) che può fare e vedere tutto e il recepionist(inteso come il personale che ci lavora) che non può accedere 
alla creazione di camere e sevizi ,cosa che può fare solo l'admin.Può però aggiornare/aggiungere/cancellare un cliente e fare le crud sulle prenotazioni.
Nella tabella prenotazioni , ho previsto un tastino "checkout" dove c'è il riepilogo della prenotazione, l'importo di quanto deve essere ancora versato e un tastino con un download pdf ( che scarica quindi un file pdf con il riepilogo, ho pensato eventualmente da stampare e da dare al "cliente").
Nel metodo per il download del file,dentro a prenotazioniController, ho specificato quale pacchetto ho scaricato per implementare la funzione di download.

Se si vuole testare sul web, consiglio di loggarsi direttamente come admin in modo che si ha accesso a tutto.
Le query utilizzate per la creazione delle tabelle e per il popolamento di giusto un paio di record ciascuno, le ho inserite in un file dentro la cartella SQL.
Ad ogni modo per l'accesso i dati sono :
admin/adminpass oppure receptionist/receptionpass (rispettivamente username/password di accesso).

Nell'ultimo link , ovvero "ricerca", ci sono le chiamate con ajax come richiesto dall'esercizio per la ricerca tramite il codice fiscale e per le prenotazioni con "pensione completa" come parametro .

Il codice è organizzato in models,Dao e controllers(ogni Dao ha il suo controller).
Nella cartella Service, ho implementato solo il solito sistema di auth .

La cartella views è organizzata per sotto-cartelle. Ogni sotto-cartella ha il proprio cshtml per l'index della pagina,la creazione,il dettaglio , il modifica.

Sul mio pd funziona tutto , spero di aver fatto il committ bene di tutto il progetto. Nel caso ci fossero problemi, magari ci sentiamo .
Grazie mille
