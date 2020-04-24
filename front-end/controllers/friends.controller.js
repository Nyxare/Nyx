const linksCtrl = {};

const pool = require('../database');

linksCtrl.renderAddFriend = (req, res) => {
    res.render('friends/addfriend');
};

linksCtrl.addFriend = async (req, res) => {
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

linksCtrl.renderLinks = async (req, res) => {
    const links = await pool.query('SELECT * FROM Directories WHERE userID = ?', [req.user.id]);
    res.render('links/list', { links });
}

linksCtrl.deleteLink = async (req, res) => {
    const { id } = req.params;
    await pool.query('DELETE FROM Directories WHERE connectionID = ?', [id]);
    req.flash('success', 'Link Removed Successfully');
    res.redirect('/list');
};

linksCtrl.renderEditLink = async (req, res) => {
    const { id } = req.params;
    const links = await pool.query('SELECT * FROM Directories WHERE connectionID = ?', [id]);
    console.log(links);
    res.render('list/edit', {link: links[0]});
};

linksCtrl.editLink = async (req,res) => {
    const { id } = req.params;
    const { url} = req.body; 
    const newLink = {
        url
    };
    await pool.query('UPDATE Directories set ? WHERE connectionID = ?', [newLink, id]);
    req.flash('success', 'Link Updated Successfully');
    res.redirect('/links');
}

module.exports = linksCtrl;