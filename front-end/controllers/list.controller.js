const linksCtrl = {};

const pool = require('../database');

linksCtrl.renderAddDirectory = (req, res) => {
    res.render('list/add');
};

linksCtrl.addDirectory = async (req, res) => {
    const {url} = req.body;

    const newLink = {
        id: req.user.id,
        url,
        connectionID: id+url
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