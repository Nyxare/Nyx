"use scrict";
const linksCtrl = {};

const pool = require('../database');
const fetch = require("node-fetch");

linksCtrl.renderAddDirectory = (req, res) => {
    res.render('list/add');
};

linksCtrl.addDirectory = async (req, res) => {

    let jsonfolders, jsonfiles = [];
    let mainObject = {};

    let showObject = function() {
        for (let prop in mainObj) {
            console.log(prop);
            console.log(mainObject[prop]);
        };
    }

    fetch("./data.json")
    .then(function(resp){
        return resp.json();
    })
    .then(function(data){
        console.log(data);
        jsonfolders = data.Directorios;
        jsonfiles = data.Files;
        mainObject = data;
        showObject();
    });

    const newLink = {
        id: req.user.id,
        userJSON
    };
    await pool.query('INSERT INTO Directories set ?', [newLink]);
    req.flash('success', 'Link Saved Successfully');
    res.redirect('/list');
}

linksCtrl.renderDirectory = async (req, res) => {
    const links = await pool.query('SELECT * FROM Directories WHERE userID = ?', [req.user.id]);
    res.render('list/list', { links });
}

module.exports = linksCtrl;