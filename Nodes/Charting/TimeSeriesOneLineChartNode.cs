using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using QuickChart;


namespace NodeBlock.Plugin.Charting.Nodes.Charting
{
    [NodeDefinition("TimeSeriesOneLineChartNode", "Generate Time Series One Line Chart", NodeTypeEnum.Function, "Charting")]
    [NodeGraphDescription("Generate image from time series data with one line")]

    public class TimeSeriesOneLineChartNode : Node
    {
        public TimeSeriesOneLineChartNode(string id, BlockGraph graph)
           : base(id, graph, typeof(TimeSeriesOneLineChartNode).Name)
        {
            this.InParameters.Add("xDataSeries", new NodeParameter(this, "xDataSeries", typeof(string), true));
            this.InParameters.Add("yDataSeries", new NodeParameter(this, "yDataSeries", typeof(string), true));
            this.InParameters.Add("chartLabel", new NodeParameter(this, "chartLabel", typeof (string), true));
            this.InParameters.Add("dataLabel", new NodeParameter(this, "dataLabel", typeof(string), true));

            this.OutParameters.Add("chartUrl", new NodeParameter(this, "chartUrl", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var xData = this.InParameters["xDataSeries"].GetValue().ToString();
            var yData = this.InParameters["yDataSeries"].GetValue().ToString();
            var chartLabel = this.InParameters["chartLabel"].GetValue().ToString();
            var dataLabel = this.InParameters["dataLabel"].GetValue().ToString();

            Chart qc = new Chart();

            qc.Width = 600;
            qc.Height = 360;
 
            var qcConfig = @"{ type: 'line', data: { labels: ";
            qcConfig += xData;
            qcConfig += ", datasets: [{ label: '";
            qcConfig += dataLabel;
            qcConfig += "', data: ";
            qcConfig += yData;
            qcConfig += " } ] }, ";
            
            qcConfig += "options: { responsive: true, title: { display: true, text: '";
            qcConfig += chartLabel;
            qcConfig += "' },";

            qcConfig += "scales: { ";

            qcConfig += "XAxes: [{ display: false, type: 'time', ";
            qcConfig += "time: { parser: 'YYYY-MM-DD HH:mm:ss' }, ";
            qcConfig += "scaleLabel: { display: false, labelString: 'Date' }, ";
            qcConfig += "ticks: { major: { enabled: true } }";
            qcConfig += "}], ";

            qcConfig += "YAxes: [{ display: true, ";
            qcConfig += "scaleLabel: { display: true, labelString: 'Price' } ";
            qcConfig += "}]";

            qcConfig += "} } }";

            qc.Config = qcConfig;

            string sUrl = qc.GetShortUrl();

            this.OutParameters["chartUrl"].SetValue(sUrl).ToString();

            return true;
        }
    }
}
