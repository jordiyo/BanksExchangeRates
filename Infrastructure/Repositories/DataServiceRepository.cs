﻿using BanksExchangeRates.Domain.Entities;
using BanksExchangeRates.Domain.Interfaces;
using BanksExchangeRates.Util;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Chrome;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace BanksExchangeRates.Infrastructure.Repositories
{
    public class DataServiceRepository : IDataService
    {
        private readonly List<XPathModel> _xPathModels;

        public DataServiceRepository(IOptions<List<XPathModel>> xPathModels) 
        {
            _xPathModels = xPathModels.Value;
        }

        public async Task<List<BanksExchangeRatesModel>> GetUpdatedDataAsync()
        {
            var banksExchangeRatesModels = new List<BanksExchangeRatesModel>();

            var samplePages = new List<string> {
                new SamplePage().CBE_EXCHANGE_RATE_PAGE,
                new SamplePage().OROMIA_EXCHANGE_RATE_PAGE,
                new SamplePage().BUNA_EXCHANGE_RATE_PAGE,
                new SamplePage().DASHEN_EXCHANGE_RATE_PAGE,
                new SamplePage().ZEMEN_EXCHANGE_RATE_PAGE,
                new SamplePage().AMHARA_EXCHANGE_RATE_PAGE,
                new SamplePage().COOP_EXCHANGE_RATE_PAGE,
                new SamplePage().NIB_EXCHANGE_RATE_PAGE,
                new SamplePage().ADDIS_EXCHANGE_RATE_PAGE,
                new SamplePage().GLOBAL_EXCHANGE_RATE_PAGE,
                new SamplePage().AWASH_EXCHANGE_RATE_PAGE,
                new SamplePage().HIBRET_EXCHANGE_RATE_PAGE,
                new SamplePage().TSEHAY_EXCHANGE_RATE_PAGE,
                new SamplePage().TSEDEY_EXCHANGE_RATE_PAGE,
                new SamplePage().SIINQEE_EXCHANGE_RATE_PAGE,
                new SamplePage().GADAA_EXCHANGE_RATE_PAGE,
                new SamplePage().GOHBETOCH_EXCHANGE_RATE_PAGE
                };

            foreach (var (xPathModel, index) in _xPathModels.Select((value, index) => (value, index)))
            {
                var document = new FetchWebPage().FecthWebPage(xPathModel.ExchangeRateWebPageUrl).Result;
                var exchangeRateScraperRepository = new ExchangeRateScraperRepository();
                var x = new HtmlDocument();
                x.LoadHtml(document);
                var banksExchangeRatesModel = exchangeRateScraperRepository.scrapeExchangeRate(x, xPathModel);
                banksExchangeRatesModels.Add(banksExchangeRatesModel);
            }

            return await Task.FromResult(banksExchangeRatesModels);
        }
    }
}
