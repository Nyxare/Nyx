const express = require('express');
const router = express.Router();

const { isLoggedIn } = require('../lib/auth');

const { renderAddDirectory, addDirectory, renderDirectory, uploadFile, renderUploadFile, downloadFile } = require('../controllers/list.controller')

// Authorization
router.use(isLoggedIn);

// Routes
router.get('/add', renderAddDirectory);
router.post('/add', addDirectory);
router.get('/', isLoggedIn, renderDirectory);



router.post('/upload', uploadFile);
router.get('/convert', renderUploadFile);
router.get('/download', downloadFile);

module.exports = router;