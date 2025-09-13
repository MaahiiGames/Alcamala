import { initializeApp } from 'https://www.gstatic.com/firebasejs/12.2.1/firebase-app.js';
import { getAuth, createUserWithEmailAndPassword, signInWithEmailAndPassword, signOut, onAuthStateChanged } from 'https://www.gstatic.com/firebasejs/12.2.1/firebase-auth.js';

const firebaseConfig = {
    apiKey: "AIzaSyCKwtEKLNo_svyduP_LhfgYah2yCCupRzs",
    authDomain: "alcamala-firebase.firebaseapp.com",
    databaseURL: "https://alcamala-firebase-default-rtdb.europe-west1.firebasedatabase.app",
    projectId: "alcamala-firebase",
    storageBucket: "alcamala-firebase.firebasestorage.app",
    messagingSenderId: "961717907605",
    appId: "1:961717907605:web:37f47eae4c67d6e0f12b9c",
    measurementId: "G-918DC7JXKZ"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Firebase Authentication and get a reference to the service
const auth = getAuth(app);

onAuthStateChanged(auth, (user) => {
    if (user) {
        // User is signed in, see docs for a list of available properties
        // https://firebase.google.com/docs/reference/js/auth.user
        //const uid = user.uid;
        // ...
        var userJson = JSON.stringify(user);
        Firebase.onAuthStateChanged(userJson);
    } else {
        // User is signed out
        // ...
        Firebase.onAuthStateChanged(null);
    }
});

class Firebase {
    static firebaseService;

    static async initializeFirebaseService(firebaseServiceReference) {
        this.firebaseService = firebaseServiceReference;
    }

    static async onAuthStateChanged(user) {
        if (this.firebaseService == null) return;

        this.firebaseService.invokeMethodAsync("OnAuthStateChanged", user)
    }

    static async trySignInWithEmailAndPassword(email, password) {
        return new Promise((resolve, reject) => {
            signInWithEmailAndPassword(auth, email, password)
                .then((userCredential) => {
                    // Signed in 
                    const user = userCredential.user;
                    // ...
                    resolve("success");
                })
                .catch((error) => {
                    const errorCode = error.code;
                    const errorMessage = error.message;
                    resolve(errorMessage);
                });
        });
    }

    static signOut() {
        signOut(auth);
    }

    static tryCreateUser(email, password) {
        return new Promise((resolve, reject) => {
            createUserWithEmailAndPassword(auth, email, password)
                .then((userCredential) => {
                    // Signed up 
                    const user = userCredential.user;
                    // ...
                    resolve("success");
                })
                .catch((error) => {
                    const errorCode = error.code;
                    const errorMessage = error.message;
                    // ..
                    resolve(errorMessage);
                });
        });
    }
}

window.Firebase = Firebase;