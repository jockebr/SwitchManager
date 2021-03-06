﻿using SwitchManager.nx.library;
using SwitchManager.nx.cdn;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchManager.nx.library
{
    internal class SwitchCollection
    {
        public ObservableCollection<SwitchCollectionItem> Collection { get; set; }
        private CDNDownloader loader;

        internal SwitchCollection(CDNDownloader loader)
        {
            Collection = new ObservableCollection<SwitchCollectionItem>();
            this.loader = loader;
        }

        internal SwitchCollectionItem AddGame(string name, string titleid, string titlekey, SwitchCollectionState state, bool isFavorite)
        {
            SwitchCollectionItem item = new SwitchCollectionItem(name, titleid, titlekey, state, isFavorite);
            Collection.Add(item);
            return item;
        }

        internal SwitchCollectionItem AddGame(string name, string titleid, string titlekey, bool isFavorite)
        {
            SwitchCollectionItem item = new SwitchCollectionItem(name, titleid, titlekey, isFavorite);
            Collection.Add(item);
            return item;
        }

        internal SwitchCollectionItem AddGame(string name, string titleid, string titlekey, SwitchCollectionState state)
        {
            SwitchCollectionItem item = new SwitchCollectionItem(name, titleid, titlekey, state);
            Collection.Add(item);
            return item;
        }

        internal SwitchCollectionItem AddGame(string name, string titleid, string titlekey)
        {
            SwitchCollectionItem item = new SwitchCollectionItem(name, titleid, titlekey);
            Collection.Add(item);
            return item;
        }

        internal void LoadTitleIcons(string localPath)
        {
            foreach (SwitchCollectionItem item in Collection)
            {
                item.Game.Icon = LoadTitleIcon(item.Game);
            }
        }

        /// <summary>
        /// Gets a title icon. If it isn't cached locally, gets it from nintendo.
        /// TODO: Remote and local paths are currently hard-coded in the image loader!
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        private SwitchImage LoadTitleIcon(SwitchGame game)
        {
            SwitchImage img = loader.GetLocalImage(game.TitleID);
            if (img == null)
            {
                // Ask the image loader to get the image remotely and cache it
                img = loader.GetRemoteImage(game);
            }

            // Return cached image
            return img;
        }

        public void LoadTitleKeysFile(string filename)
        {
            var lines = File.ReadLines(filename);
            foreach (var line in lines)
            {
                string[] split = line.Split('|');
                string tid = split[0]?.Trim()?.Substring(0, 16);
                string tkey = split[1]?.Trim()?.Substring(0, 32);
                string name = split[2]?.Trim();

                var game = AddGame(name, tid, tkey)?.Game;
                if (game != null)
                {
                    //List<string> versions = loader.GetVersions(game);
                    //foreach (var v in versions) game.Versions.Add(v);
                }
            }
        }
    }
}
