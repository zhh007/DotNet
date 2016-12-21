requirejs.config({
    baseUrl: '.',
    paths: {
        'jquery': '../scripts/jquery-1.10.2',
        'signalr': '../scripts/jquery.signalR-2.2.1',
        'knockout': '../scripts/knockout-3.4.0',
        'smoothie': '../scripts/smoothie',
        'signalr.hubs': '../SignalR/hubs?'
    },
    shim: {
        "jquery": { exports: "$" },
        "knockout": { exports: "ko" },
        "signalr": { deps: ["jquery"] },
        "signalr.hubs": { deps: ["signalr"] }
    }
});

if (document.createElement('canvas').getContext) {
    requirejs(['knockout', 'smoothie', 'signalr.hubs'], function (ko) {
        var perfHub = $.connection.perfHub;

        perfHub.client.newMessage = function (message) {
            model.addMessage(message);
        };

        perfHub.client.newCounters = function (counters) {
            model.addCounters(counters);
        };

        $.connection.hub.logging = true;
        $.connection.hub.start();

        var Model = function () {
            var self = this;
            self.message = ko.observable("");
            self.messages = ko.observableArray();
            self.counters = ko.observableArray();
        };

        var ChartEntry = function (name) {
            var self = this;
            self.name = name;
            self.chart = new SmoothieChart({ millisPerPixel: 50, labels: { fontSize: 15 } });
            self.timeSeries = new TimeSeries();
            self.chart.addTimeSeries(self.timeSeries, { lineWidth: 3, strokeStyle: "#00ff00" });
        }

        ChartEntry.prototype = {
            addValue: function (value) {
                var self = this;
                self.timeSeries.append(new Date().getTime(), value);
            },
            start: function () {
                var self = this;
                self.canvas = document.getElementById(self.name);
                self.chart.streamTo(self.canvas);
            }
        };

        Model.prototype = {
            sendMessage: function () {
                var self = this;
                perfHub.server.send(self.message());
                self.message("");
            },
            addMessage: function (message) {
                var self = this;
                self.messages.push(message);
            },
            addCounters: function (updatedCounters) {
                var self = this;
                $.each(updatedCounters, function (i, o) {
                    var entry = ko.utils.arrayFirst(self.counters(), function (counter) {
                        return counter.name == o.name;
                    });
                    if (!entry) {
                        entry = new ChartEntry(o.name);
                        self.counters.push(entry);
                        entry.start();
                    }
                    entry.addValue(o.value);
                });
            }
        };

        var model = new Model();

        $(function () {
            ko.applyBindings(model);
        });

    });
}