

const friendsCtrl = {};

const pool = require('../database');
const app = require('express')()
const bodyParser = require('body-parser')
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: true }))

friendsCtrl.renderAddFriend = (req, res) => {
    res.render('friends/addfriend');
};

friendsCtrl.addFriend = async (req, res) => {  
    var friendget = req.body.User2;
    User2 = await pool.query('SELECT userID FROM Usuario WHERE username = ?', [friendget]);
    console.log(User2[0].userID)
    const newFriend = {
        User1: req.user.userID,
        User2: User2[0].userID
    };
    await pool.query('INSERT INTO Friends set ?', [newFriend]);
    req.flash('success', 'Friend Added Successfully');
    res.redirect('/friends');
}

friendsCtrl.renderFriends = async (req, res) => {
    //const friends = await pool.query('SELECT User2 FROM Friends WHERE User1 = ?', [req.user.userID]);
    const friends = await pool.query('SELECT username FROM Friends, Usuario WHERE User2 = userID AND User1 = ?', [req.user.userID]);
    //console.log(friends);
    res.render('friends/friends', { friends });
}


friendsCtrl.deleteFriend = async (req, res) => {
    var friend = req.params;
    //console.log(friend)
    var friend2delete = await pool.query('SELECT userID FROM Usuario WHERE username = ?', [friend.username]);
    //console.log(friend2delete[0].userID);
    var deleter = {
        User1: req.user.userID,
        User2: friend2delete[0].userID,
        
    }
    //console.log(deleter);

    const finale = await pool.query(' DELETE FROM Friends WHERE User1 = ? AND User2 = ?', [deleter.User1, deleter.User2]);
    console.log(finale);
    req.flash('success', 'Friend Removed Successfully');
    res.redirect('/friends');
};


module.exports = friendsCtrl;