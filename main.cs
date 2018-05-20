using Pipliz.JSON;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace BrightExistence.SimpleTools
{
    [ModLoader.ModManager]
    public static class Main
    {
        const string NAMESPACE = "BrightExistence.SimpleTools";

        /// <summary>
        /// OnAssemblyLoaded callback entrypoint. Used for mod configuration / setup.
        /// </summary>
        /// <param name="path">The starting point of mod file structure.</param>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, NAMESPACE + ".OnAssemblyLoaded")]
        public static void OnAssemblyLoaded(string path)
        {
            // Announce ourselves.
            Pipliz.Log.Write("{0} loading.", NAMESPACE);
            Pipliz.Log.Write("Built using SimpleTools version {0}", Variables.toolkitVersion);

            // Get a properly formatted version of our mod directory.
            Variables.ModGamedataDirectory = Path.GetDirectoryName(path).Replace("\\", "/");
        }

        /// <summary>
        /// AfterSelectedWorld callback entry point. Used for adding textures.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterSelectedWorld, NAMESPACE + ".afterSelectedWorld"), ModLoader.ModCallbackProvidesFor("pipliz.server.registertexturemappingtextures")]
        public static void afterSelectedWorld()
        {
            // ---------------AUTOMATED TEXTURE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning texture loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateTextureObjects();
            List<SpecificTexture> AutoTextures = new List<SpecificTexture>();
            foreach (SpecificTexture Tex in Variables.SpecificTextures) AutoTextures.Add(Tex);
            foreach (SpecificTexture thisTexture in AutoTextures) thisTexture.registerTexture();
            Pipliz.Log.Write("{0}: Texture loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// The afterAddingBaseTypes entrypoint. Used for adding blocks.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterAddingBaseTypes, NAMESPACE == null ? "" : NAMESPACE + ".afterAddingBaseTypes")]
        public static void afterAddingBaseTypes(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            Variables.itemsMaster = items;

            // ---------------(AUTOMATED BLOCK REGISTRATION)---------------
            Pipliz.Log.Write("{0}: Beginning Item loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateItemObjects();
            List<SimpleItem> AutoItems = new List<SimpleItem>();
            foreach (SimpleItem Item in Variables.Items) AutoItems.Add(Item);
            foreach (SimpleItem Item in AutoItems) Item.registerItem(items);
            Pipliz.Log.Write("{0}: Item loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// The afterItemType callback entrypoint. Used for registering jobs and recipes.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterItemTypesDefined")]
        public static void AfterItemTypesDefined()
        {
            //---------------AUTOMATED RECIPE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Recipe loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateRecipeObjects();
            foreach (SimpleRecipe Rec in Variables.Recipes) Rec.addRecipeToLimitType();
            Pipliz.Log.Write("{0}: Recipe and Job loading complete.", NAMESPACE == null ? "" : NAMESPACE);

            //---------------AUTOMATED INVENTORY BLOCK REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning crate registration.", NAMESPACE == null ? "" : NAMESPACE);
            foreach (SimpleItem Item in Variables.Items) Item.registerAsCrate();
            Pipliz.Log.Write("{0}: Crate registration complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// AfterDefiningNPCTypes callback. Used for registering jobs.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterDefiningNPCTypes")]
        [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.jobs.resolvetypes")]
        public static void AfterDefiningNPCTypes()
        {
            // ---------------AUTOMATED JOBS REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Job loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateJobs();
            Pipliz.Log.Write("{0}: Job loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAddResearchables, NAMESPACE == null ? "" : NAMESPACE + ".OnAddResearchables")]
        public static void OnAddResearchables()
        {
            //---------------AUTOMATED RESEARCHABLE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Research loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateResearchObjects();
            foreach (SimpleResearchable R in Variables.Researchables)
            {
                R.Register();
            }
            Pipliz.Log.Write("{0}: Research loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }
    }
}
