namespace MagicConsole
{
    public enum CommandTypes { Exit, Help, Custom }
    public enum MenuType 
    { 
        Standard,       //Il menu dei comandi è un classico elenco numerato e per selezionare un opzione bisogna digitare il numero
        Advanced        //Il menu dei comandi è grafico e per selezionare un opzione bisogna scorrere su e giu e fare invio sull'elemento selezionato
    }
    public enum ExecutionMode
    {
        Silent,         //Il programma non ha un menu ma ha una serie di parametri da lanciare da riga di comando
        Interactive,    //Il programma ha un menu di comandi e delle opzioni che devono essere selezionate dall'utente
        Smart           //Il programma ha un menu ma se vengono passati i parametri da riga di comando va in modalità silent
    }
    public enum OptionType
    {
        Flag, Date, Time, Enum, Integer, Decimal, String
    }
    public enum InfoItem
    {
        fullpath, filename, title, description, company, product, copyright, trademark, assemblyVersion, fileVersion, guid, language
    }

}