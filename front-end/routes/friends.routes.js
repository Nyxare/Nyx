const express = require('express');
const router = express.Router();

const { isLoggedIn } = require('../lib/auth');

const { renderAddFriend, addFriend, renderFriends, deleteFriend } = require('../controllers/friends.controller')

// Authorization
router.use(isLoggedIn);

// Routes
router.get('/addfriend', renderAddFriend);
router.post('/addfriend', addFriend);
router.get('/', isLoggedIn, renderFriends);
router.get('/delete/:username', deleteFriend);


module.exports = router;