using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Classes
{
    public class ArquivoLog
    {
        private Classes.Objetos.ArenaObjItens ArenaObjItens = new Classes.Objetos.ArenaObjItens();
        //public string Path = @"E:\domains\dimensionallegends.com\wwwroot\Log\ArenaState\";
        public string Path = @"C:\Users\teste\Documents\Visual Studio 2013\Projects\DimensionalLegendsAtual\trunk\DimensionalLegends\Log\ArenaState\";
        public string Name = "";

        public string Select(string nomeArquivo = "")
        {
            Name = nomeArquivo != "" ? nomeArquivo : Name;
            
            String dadosArquivo = "";

            using (StreamReader sr = new StreamReader(Path + string.Concat(Name, ".txt")))
            {
                dadosArquivo = sr.ReadToEnd();
            }

            return dadosArquivo;
        }

        public void Update(string nomeArquivo = "")
        {
            Name = nomeArquivo != "" ? nomeArquivo : Name;

            using (StreamWriter newTask = new StreamWriter(Path + string.Concat(Name, ".txt"), false))
            {
                newTask.WriteLine(JsonConvert.SerializeObject(ArenaObjItens));
            }
        }

        public void Update(List<Objetos.Card> p1deckListCard, List<Objetos.Card> p2deckListCard, Objetos.CardBloco[] arenaY1, Objetos.CardBloco[] arenaY2, Objetos.CardBloco[] arenaY3, Objetos.CardBloco[] arenaY4, Objetos.ArenaConfig config)
        {
            if (Name == "") return;

            ArenaObjItens.p1deckListCard = p1deckListCard;
            ArenaObjItens.p2deckListCard = p2deckListCard;
            ArenaObjItens.arenasituacaoY1 = arenaY1;
            ArenaObjItens.arenasituacaoY2 = arenaY2;
            ArenaObjItens.arenasituacaoY3 = arenaY3;
            ArenaObjItens.arenasituacaoY4 = arenaY4;
            ArenaObjItens.arenaConfig = config;

            using (StreamWriter newTask = new StreamWriter(Path + string.Concat(Name, ".txt"), false))
            {
                newTask.WriteLine(JsonConvert.SerializeObject(ArenaObjItens));
            }
        }

        public ArquivoLog()
        {

        }

        public ArquivoLog(string nomeArquivo)
        {
            Name = nomeArquivo;
        }

        public ArquivoLog(List<Objetos.Card> p1deckListCard, List<Objetos.Card> p2deckListCard, Objetos.CardBloco[] arenaY1, Objetos.CardBloco[] arenaY2, Objetos.CardBloco[] arenaY3, Objetos.CardBloco[] arenaY4, Objetos.ArenaConfig config)
        {
            ArenaObjItens.p1deckListCard = p1deckListCard;
            ArenaObjItens.p2deckListCard = p2deckListCard;
            ArenaObjItens.arenasituacaoY1 = arenaY1;
            ArenaObjItens.arenasituacaoY2 = arenaY2;
            ArenaObjItens.arenasituacaoY3 = arenaY3;
            ArenaObjItens.arenasituacaoY4 = arenaY4;
            ArenaObjItens.arenaConfig = config;
        }
    }


}
