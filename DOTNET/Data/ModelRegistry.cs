using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DOTNET;

namespace DOTNET.Data
{
    /// <summary>
    /// Singleton for storing references to specific object mapper objects.
    /// </summary>
    /// <summary>
    /// Singleton for storing references to specific object mapper objects.
    /// </summary>
    public class ModelRegistry : DOTNET.DataSource.IDataSource
    {
        private static ModelRegistry _instance = new ModelRegistry();
        private IDictionary _models = new Hashtable();

        public ModelRegistry()
        {
            DOTNET.DataSource.Init(this);
        }

        public void Load(IModelObject obj)
        {
            Model(obj.GetType()).Load(obj);
        }

        public void Log(IModelObject obj)
        {
            Model(obj.GetType()).Log(obj);
        }

        public static Model Model(Type type)
        {
            Model m = (Model)_instance._models[type];

            //if (m == null)
            //{
                m = LoadModel(type);
                _instance._models[type] = m;
            //}

            return m;
        }

        public static Model<T> Model<T>() where T : IModelObject, new()
        {
            return (Model<T>)Model(typeof(T));
        }

        private static Model LoadModel(Type type)
        {
            Model m = null;

            switch (type.Name)
            {
                //Need one case block per domain object
                case "Applicant": m = new MApplicant(); break;
                case "Bank": m = new MBank(); break;
                case "ApplicantRequest": m = new MApplicantRequest(); break;
                case "Brand": m = new MBrand(); break;
                case "Campaign": m = new MCampaign(); break;
                case "CardDesign": m = new MCardDesign(); break;
                case "Certificate": m = new MCertificate(); break;
                case "MarketingCode": m = new MMarketingCode(); break;
                case "MLP": m = new MMLP(); break;
                case "PreScreen": m = new MPreScreen(); break;
                case "Pricing": m = new MPricing(); break;
                case "Recipient": m = new MRecipient(); break;
                case "Term": m = new MTerm(); break;
            }

            return m;
        }

        private static Model<T> LoadMapper<T>() where T : IModelObject, new()
        {
            return Model<T>();
        }
    }
}
