/// <reference path="help.ts" />
/// <reference path="data.ts" />

function setupFrontlineApp() {
    setupEarlyErrors();

    var mapInstance = setupMap();

    var oms = new OverlappingMarkerSpiderfier(mapInstance, { keepSpiderfied: true });

    var tweetsContainer = document.getElementById("needs");

    var browserIsIe = isIe();
     
    if (!browserIsIe) {
        ["1250844201203400705", "1250736227395022848", "1250539285603733505", "1250209322056978434"]
            .forEach(function (tweetId) {
                addTweet(tweetId, tweetsContainer);
            });
    }

    function createClusterIcon(baseClass) {
        return function (cluster) {
            var childCount = cluster.getChildCount();
            var c = ' marker-cluster-';
            if (childCount < 10) {
                c += 'small';
            } else if (childCount < 100) {
                c += 'medium';
            } else {
                c += 'large';
            }
            return new L.DivIcon({ html: '<div><span>' + childCount + '</span></div>', className: baseClass + ' marker-cluster' + c, iconSize: new L.Point(40, 40) });
        }
    }


    //TODO:HV 
    var mapboxTitlesLayer = L.tileLayer(`https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=${window.appConfig.mapboxToken}`, {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, ' +
            '<a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
            'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        maxZoom: 18
    });

    mapboxTitlesLayer.addTo(mapInstance); 

    var needsPoints = L.markerClusterGroup({
        iconCreateFunction: createClusterIcon('needs_cluster')
    });

    var needsMetPoints = L.markerClusterGroup({
        iconCreateFunction: createClusterIcon('needsMet_cluster')
    });

    var suppliesPoints = L.markerClusterGroup({
        iconCreateFunction: createClusterIcon('supplies_cluster')
    });

    var needsBreakdownChildren = [];
    var needsMetBreakdownChildren = [];
    var suppliesBreakdownChildren = [];


    var needsOpen = true;
    var needsMetOpen = false;
    var suppliesOpen = false;

    Data.get().then(function (d) {

        //needs
        d.needs.posts.forEach(function (np) {
            var marker = L.marker(np.location, { icon: postIcon(Settings.needsColor) })
                .bindPopup(np.popupHtml);
            needsPoints.addLayer(marker);
            oms.addMarker(marker);
        });

        needsPoints.addTo(mapInstance); //default

        d.needs.pointsList.forEach(function (npt) {

            var ppeTypeMarkerClusterGroup = L.markerClusterGroup({
                iconCreateFunction: createClusterIcon('needs_cluster')
            });

            npt.points.forEach(function (p) {
                var marker = L.marker(p.location, {
                    icon: pointIcon(Settings.needsColor, npt.class)
                }).bindPopup(p.popupHtml);

                ppeTypeMarkerClusterGroup.addLayer(marker);
                oms.addMarker(marker);
            });

            needsBreakdownChildren.push({
                label: Help.coloredNumberedLabel(npt.ppeType, npt.points.length, npt.class)
                , layer: ppeTypeMarkerClusterGroup
            });
        });



        //needs Met
        d.needsMet.posts.forEach(function (np) {
            var marker = L.marker(np.location, { icon: postIcon(Settings.needsMetColor) })
                .bindPopup(np.popupHtml);
            needsMetPoints.addLayer(marker);
            oms.addMarker(marker);
        });

        d.needsMet.pointsList.forEach(function (npt) {

            var ppeTypeMarkerClusterGroup = L.markerClusterGroup({
                iconCreateFunction: createClusterIcon('needsMet_cluster')
            });

            npt.points.forEach(function (p) {
                var marker = L.marker(p.location, {
                    icon: pointIcon(Settings.needsMetColor, npt.class)
                }).bindPopup(p.popupHtml);

                ppeTypeMarkerClusterGroup.addLayer(marker);
                oms.addMarker(marker);
            });

            needsMetBreakdownChildren.push({
                    label: Help.coloredNumberedLabel(npt.ppeType, npt.points.length, npt.class)
                    , layer: ppeTypeMarkerClusterGroup
            });

        });


        //supplies
        d.supplies.posts.forEach(function (sp) {
            var marker = L.marker(sp.location, { icon: postIcon(Settings.suppliesColor) })
                .bindPopup(sp.popupHtml);
            suppliesPoints.addLayer(marker);
            oms.addMarker(marker);
        });

        d.supplies.pointsList.forEach(function (spl) {

            var ppeTypeMarkerClusterGroup = L.markerClusterGroup({
                iconCreateFunction: createClusterIcon('supplies_cluster')
            });

            spl.points.forEach(function (p) {

                var marker = L.marker(p.location, {
                    icon: pointIcon(Settings.suppliesColor, spl.class)
                }).bindPopup(p.popupHtml);

                ppeTypeMarkerClusterGroup.addLayer(marker);
                oms.addMarker(marker);
            });

            suppliesBreakdownChildren.push({
                label: Help.coloredNumberedLabel(spl.ppeType, spl.points.length, spl.class)
                , layer: ppeTypeMarkerClusterGroup
            });
        });

        var overlayTree = [
            {
                label: "<div class='toggleGroup needs_group'>Needs<i class='fas fa-chevron-circle-up'></i></div>" // 'Needs'
                , eventedClasses: [
                    {
                        className: "toggleGroup"
                        , event: "click"
                        , selectAll: toggleNeeds
                    },
                    { //clicking posts -- deselects all breakdown chbs 
                        className: "leaflet-control-layers-selector" // rb = Posts
                        , event: "change"
                        , selectAll: postsChangeHandler
                    }
                ]
                , children: [
                    { label: Help.numberedLabel('Posts', d.needs.posts.length), radioGroup: "NeedsChoice", layer: needsPoints },
                    {
                        label: Help.numberedLabel('Breakdown', d.needs.pointsCount)
                        , radioGroup: "NeedsChoice"
                        , collapsed: true
                        , children: needsBreakdownChildren
                        , layer: L.layerGroup([]) //needed to make rb 
                        , eventedClasses: {
                            className: "leaflet-control-layers-selector" //rb = breakdown 
                            , event: "change"
                            , selectAll: breakdownChangeHandler
                        }
                    }
                ]
            }
            ,
            {
                label: "<div class='toggleGroup needsMet_group'>Needs Met<i class='fas fa-chevron-circle-up'></i></div>" // 'Needs Met'
                , eventedClasses: [
                    {
                        className: "toggleGroup"
                        , event: "click"
                        , selectAll: toggleNeedsMet
                    },
                    { //clicking posts -- deselects all breakdown chbs 
                        className: "leaflet-control-layers-selector" // rb = Posts
                        , event: "change"
                        , selectAll: postsChangeHandler
                    }
                ]
                , children: [
                    { label: Help.numberedLabel('Posts', d.needsMet.posts.length), radioGroup: "NeedsMetChoice", layer: needsMetPoints },
                    {
                        label: Help.numberedLabel('Breakdown', d.needsMet.pointsCount)
                        , radioGroup: "NeedsMetChoice"
                        , collapsed: true
                        , children: needsMetBreakdownChildren
                        , layer: L.layerGroup([]) //needed to make rb 
                        , eventedClasses: {
                            className: "leaflet-control-layers-selector" //rb = breakdown 
                            , event: "change"
                            , selectAll: breakdownChangeHandler
                        }
                    }
                ]
            },
            {
                label: "<div class='toggleGroup supplies_group toggle_collapsed'>Supplies<i class='fas fa-chevron-circle-up'></i></div>"
                , collapsed: true
                , eventedClasses: [
                    {
                        className: "toggleGroup"
                        , event: "click"
                        , selectAll: toggleSupplies
                    },
                    { //clicking posts -- deselects all breakdown chbs 
                        className: "leaflet-control-layers-selector" // rb = Posts
                        , event: "change"
                        , selectAll: postsChangeHandler
                    }
                ]
                , children: [
                    { label: Help.numberedLabel('Posts', d.supplies.posts.length), radioGroup: "SuppliesChoice", layer: suppliesPoints },
                    {
                        label: Help.numberedLabel('Breakdown', d.supplies.pointsCount)
                        , radioGroup: "SuppliesChoice"
                        , collapsed: true
                        , children: suppliesBreakdownChildren
                        , layer: L.layerGroup([]) //needed to make rb 
                        , eventedClasses: {
                            className: "leaflet-control-layers-selector" //rb = breakdown 
                            , event: "change"
                            , selectAll: breakdownChangeHandler
                        }
                    }
                ]
            }
        ];


        L.control.layers.tree(mapboxTitlesLayer, overlayTree, { closedSymbol: '', openedSymbol: '', spaceSymbol: '', selectorBack: true, collapsed: true }).addTo(mapInstance);

    
        oms.addListener('spiderfy', function (markers) {
            //console.log("spiderfy");
            //for (var i = 0, len = markers.length; i < len; i ++) markers[i].setIcon(new lightIcon());
            //mapInstance.closePopup();
        });
        oms.addListener('unspiderfy', function (markers) {
            //console.log("unspiderfy");
            //for (var i = 0, len = markers.length; i < len; i ++) markers[i].setIcon(new darkIcon());
        });

    });

    function toggleNeeds(ev, domNode, treeNode, map) {
        var aTag = domNode.querySelectorAll("div.toggleGroup");
        var rbs = domNode.querySelectorAll("input[type='radio']");
        var children = domNode.querySelectorAll(".leaflet-layerstree-children");
        if (needsOpen) {
            rbs[1].checked = true;
            children[0].classList.add("leaflet-layerstree-hide");
            aTag[0].classList.add("toggle_collapsed");
        }
        else {
            children[0].classList.remove("leaflet-layerstree-hide");
            aTag[0].classList.remove("toggle_collapsed");
        }
        needsOpen = !needsOpen;
        return false;
    }

    function toggleNeedsMet(ev, domNode, treeNode, map) {
        var aTag = domNode.querySelectorAll("div.toggleGroup");
        var rbs = domNode.querySelectorAll("input[type='radio']");
        var children = domNode.querySelectorAll(".leaflet-layerstree-children");
        if (needsMetOpen) {
            rbs[1].checked = true;
            children[0].classList.add("leaflet-layerstree-hide");
            aTag[0].classList.add("toggle_collapsed");
        }
        else {
            children[0].classList.remove("leaflet-layerstree-hide");
            aTag[0].classList.remove("toggle_collapsed");
        }
        needsMetOpen = !needsMetOpen;
        return false;
    }

    function toggleSupplies(ev, domNode, treeNode, map) {
        var aTag = domNode.querySelectorAll("div.toggleGroup");
        var rbs = domNode.querySelectorAll("input[type='radio']");
        var children = domNode.querySelectorAll(".leaflet-layerstree-children");
        if (suppliesOpen) {
            rbs[1].checked = true;
            children[0].classList.add("leaflet-layerstree-hide");
            aTag[0].classList.add("toggle_collapsed");
        }
        else {
            children[0].classList.remove("leaflet-layerstree-hide");
            aTag[0].classList.remove("toggle_collapsed");
        }
        suppliesOpen = !suppliesOpen;
        return false;
    }

    function postsChangeHandler(ev, domNode, treeNode, map) {

        var chbs = domNode.querySelectorAll("input[type='checkbox']");
        chbs.forEach(function (ch) {
            ch.disabled = true;
        });
        var childrenWrap = domNode.querySelectorAll(".leaflet-layerstree-node");
        var toHide = childrenWrap[1].querySelectorAll(".leaflet-layerstree-children");
        toHide[0].classList.add("leaflet-layerstree-hide");
        return false;
    }

    function breakdownChangeHandler(ev, domNode, treeNode, map) {
        var chbs = domNode.querySelectorAll("input[type='checkbox']");
        chbs.forEach(function (ch) {
            ch.disabled = false;
        });
        var hiddenChildrenWrap = domNode.querySelectorAll(".leaflet-layerstree-children");
        hiddenChildrenWrap[0].classList.remove("leaflet-layerstree-hide");
    }

    function addTweet(tweetId, container) {
        var div = document.createElement("div");
        div.id = tweetId;
        div.className = "grid-item";
        container.appendChild(div);
        buildTweet(tweetId, div);
    }

    function buildTweet(tweetId, tweetContainer) {
        // Create tweet embed
        twttr.widgets.createTweet(tweetId, tweetContainer, {
            conversation: 'none'
            , width: 250
        }).then(function (tweet) {
            // inject CSS into iframe to hide elements
            var css = document.createTextNode(".EmbeddedTweet .CallToAction { display: none; } .EmbeddedTweet .TweetInfo { display: none; } .Tweet-body.e-entry-content { font-size:12px; }");
            var style;
            if (tweet.shadowRoot) {
                style = tweet.shadowRoot.firstElementChild;
            } else if(tweet.contentWindow && tweet.contentWindow.document) {
                style = tweet.contentWindow.document.querySelector('style');
            }

            if (style) {
                style.appendChild(css);
            }
            $countTweets--;
        });
    }

    function setupMap() {

        var flMap = L.map('map', { worldCopyJump: true }).setView([Settings.mapDefaultLat, Settings.mapDefaultLng], Settings.mapZoomDefault);

        //setTitleLayer(flMap);

        return flMap;
    }

    function postIcon(color) {
        color = (color && /^[a-zA-Z0-9#]+$/.test(color)) ? color : '#959595';
        size = [32, 32];
        return L.divIcon({
            className: 'custom-map-marker',
            html: '<svg class="iconic" style="fill:' + color + ';"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="marker.svg#map-marker"></use></svg>',
            iconSize: size,
            iconAnchor: [size[0] / 2, size[1]],
            popupAnchor: [0, 0 - size[1]]
        });
    }

    function pointIcon(colorPin, innerClass) {
        colorPin = (colorPin && /^[a-zA-Z0-9#]+$/.test(colorPin)) ? colorPin : '#959595';
        size = [32, 32];
        return L.divIcon({
            className: 'custom-map-marker',
            html: '<svg class="iconic" style="fill:' + colorPin + ';"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="marker.svg#map-marker"></use></svg><span class="iconic-bg ' + innerClass + '"></span>',
            iconSize: size,
            iconAnchor: [size[0] / 2, size[1]],
            popupAnchor: [0, 0 - size[1]]
        });
    }

    function setTitleLayer(map) {

        if (Settings.freeMapMode) {
            L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, &copy; <a href="https://carto.com/attribution">CARTO</a>',
                //tileSize: 512,
                //zoomOffset: -1,
                maxZoom: 18
            }).addTo(map);
        }
        else {
            L.tileLayer(`https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=${window.appConfig.mapboxToken}`, {
                attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, ' +
                    '<a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
                    'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
                id: 'mapbox/streets-v11',
                tileSize: 512,
                zoomOffset: -1,
                maxZoom: 18
            }).addTo(map);
        }
    }

    function setupEarlyErrors() {
        window.onerror = Help.handleErrors;
    }

    if (browserIsIe) {
        addIeNote(tweetsContainer);
        var twitterTimelineContainer = document.getElementById("our_voice");
        addIeNote(twitterTimelineContainer);
    }

    function addIeNote(container) {
        var div = document.createElement("div");
        div.className = "ie_note";
        div.textContent = "Internet Explorer is not supported by Twitter";
        container.appendChild(div);
    }
}

/**
 * Call to setup
 */
(function () {

    setupFrontlineApp();

})();