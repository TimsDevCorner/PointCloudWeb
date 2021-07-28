var express = require('express');
var globals = require('../public/javascripts/globals')
var router = express.Router();

/* GET home page. */
router.get('/scanner', function (req, res, next) {
  res.render('scanner', globals);
});
router.get('/map', function (req, res, next) {
  res.render('map', globals);
});
router.get('/', function (req, res, next) {
  res.render('index', globals);
});

module.exports = router;
