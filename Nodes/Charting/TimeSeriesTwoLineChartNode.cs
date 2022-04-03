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
    [NodeDefinition("TimeSeriesTwoLineChartNode", "Generate Time Series Two Line Chart", NodeTypeEnum.Function, "Charting")]
    [NodeGraphDescription("Generate image from time series data with two lines")]

    public class TimeSeriesTwoLineChartNode : Node
    {
        public TimeSeriesTwoLineChartNode(string id, BlockGraph graph)
           : base(id, graph, typeof(TimeSeriesTwoLineChartNode).Name)
        {
            this.InParameters.Add("xDataSeries", new NodeParameter(this, "xDataSeries", typeof(string), true));
            this.InParameters.Add("y1DataSeries", new NodeParameter(this, "y1DataSeries", typeof(string), true));
            this.InParameters.Add("y2DataSeries", new NodeParameter(this, "y2DataSeries", typeof(string), true));
            this.InParameters.Add("y1Label", new NodeParameter(this, "y1Label", typeof(string), true));
            this.InParameters.Add("y2Label", new NodeParameter(this, "y2Label", typeof(string), true));

            this.OutParameters.Add("chartUrl", new NodeParameter(this, "chartUrl", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var xData = this.InParameters["xDataSeries"].GetValue().ToString();
            var y1Data = this.InParameters["y1DataSeries"].GetValue().ToString();
            var y2Data = this.InParameters["y2DataSeries"].GetValue().ToString();
            var y1Label = this.InParameters["y1Label"].GetValue().ToString();
            var y2Label = this.InParameters["y2Label"].GetValue().ToString();

            Chart qc = new Chart();

            qc.Width = 600;
            qc.Height = 360;
 
            var qcConfig = @"{ type: 'line', data: { labels: ";
            qcConfig += xData;
            qcConfig += ", datasets: [{ label: '";
            qcConfig += y1Label;
            qcConfig += "', data: ";
            qcConfig += y1Data;
            qcConfig += ", fill: false, yAxisID: 'A' }, ";
            qcConfig += "{ label: '";
            qcConfig += y2Label;
            qcConfig += "', data: ";
            qcConfig += y2Data;
            qcConfig += ", fill: false, yAxisID: 'B' }";    
            qcConfig += " ] }, ";

            qcConfig += "options: { scales: { ";
            qcConfig += "yAxes: [{ id: 'A', ";
            qcConfig += "ticks: { fontColor: 'blue' } }, ";
            qcConfig += "{ id: 'B', position: 'right', ";
            qcConfig += "ticks: { fontColor: 'orange' } } ";
            qcConfig += "] } } }";

            qc.Config = qcConfig;

            string sUrl = qc.GetShortUrl();

            this.OutParameters["chartUrl"].SetValue(sUrl).ToString();

            return true;
        }
    }
}
