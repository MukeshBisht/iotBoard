/*!
 * magen-iot-admin v1.0.0
 * Copyright 2017-2018 vueghost
 * Licensed under ThemeForest License
 */

'use strict';

$(document).ready(function () {

  // displaying time and date in left sidebar
  var interval = setInterval(function () {
    var momentNow = moment();
    $('#ktDate').html(momentNow.format('MMMM DD, YYYY hh:mm:ss') + ' '
      + momentNow.format('dddd')
        .substring(0, 3).toUpperCase());
  }, 100);

  $('.kt-sideleft').perfectScrollbar({
    useBothWheelAxes: false,
    suppressScrollX: true,
    wheelPropogation: true
  });

  // hiding all sub nav in left sidebar by default.
  $('.nav-sub').slideUp();

  // showing sub navigation to nav with sub nav.
  $('.with-sub.active + .nav-sub').slideDown();

  // showing sub menu while hiding others
  $('.with-sub').on('click', function (e) {
    e.preventDefault();

    var nextElem = $(this).next();
    if (!nextElem.is(':visible')) {
      $('.nav-sub').slideUp();
    }
    nextElem.slideToggle();
  });

  // showing and hiding left sidebar
  $('#naviconMenu').on('click', function (e) {
    e.preventDefault();
    $('body').toggleClass('hide-left');
  });

  // pushing to/back left sidebar
  $('#naviconMenuMobile').on('click', function (e) {
    e.preventDefault();
    $('body').toggleClass('show-left');
  });

  // highlight syntax highlighter
  $('pre code').each(function (i, block) {
    hljs.highlightBlock(block);
  });
 // addWeather2();
}); 


function addWeather() {

  window.myWidgetParam ? window.myWidgetParam : window.myWidgetParam = [];
  window.myWidgetParam.push({ id: 15, cityid: '1273313', appid: '5dba3ee966a670d25f5408fcbf00d1d2', units: 'metric', containerid: 'openweathermap-widget-15', });
  (function () 
    { 
     var script = document.createElement('script');
     script.async = true; 
     script.charset = "utf-8"; 
     script.src = "//openweathermap.org/themes/openweathermap/assets/vendor/owm/js/weather-widget-generator.js";
     var s = document.getElementsByTagName('script')[0]; 
     s.parentNode.insertBefore(script, s);
    })();
}

function addWeather2(){
window.myWidgetParam ? window.myWidgetParam : window.myWidgetParam = [];  
window.myWidgetParam.push(
  {
    id: 11,
    cityid: '1273313',
    appid: 'b2ff4e158985c89465b68747bdc16ad8',
    units: 'metric',
    containerid: 'openweathermap-widget-15',  
  });  
  (function() {
    var script = document.createElement('script');
    script.async = true;
    script.charset = "utf-8";
    script.src = "//openweathermap.org/themes/openweathermap/assets/vendor/owm/js/weather-widget-generator.js";
    var s = document.getElementsByTagName('script')[0];
    s.parentNode.insertBefore(script, s);
    })();
}