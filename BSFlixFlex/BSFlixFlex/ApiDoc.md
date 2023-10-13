# Documentation de l'API

## Routes

### GET /api/movie/top_rated
Renvoie les films les mieux notés. 
- Paramètres : `page` (facultatif, par défaut à 1)
- Exemple : `/api/movie/top_rated?page=2`

### GET /api/tvshow/top_rated
Renvoie les émissions de télévision les mieux notées.
- Paramètres : `page` (facultatif, par défaut à 1)
- Exemple : `/api/tvshow/top_rated?page=2`

### GET /api/movie
Renvoie une liste de films.
- Paramètres : `page` (facultatif, par défaut à 1)
- Exemple : `/api/movie?page=2`

### GET /api/tvshow
Renvoie une liste d'émissions de télévision.
- Paramètres : `page` (facultatif, par défaut à 1)
- Exemple : `/api/tvshow?page=2`

### GET /api/search/movie
Recherche des films.
- Paramètres : `search` (requis), `page` (facultatif, par défaut à 1)
- Exemple : `/api/search/movie?search=inception&page=2`

### GET /api/search/tvshow
Recherche des émissions de télévision.
- Paramètres : `search` (requis), `page` (facultatif, par défaut à 1)
- Exemple : `/api/search/tvshow?search=friends&page=2`

### GET /api/movie/{id}
Renvoie les détails d'un film spécifique.
- Paramètres : `id` (requis)
- Exemple : `/api/movie/550`

### GET /api/tvshow/{id}
Renvoie les détails d'une émission de télévision spécifique.
- Paramètres : `id` (requis)
- Exemple : `/api/tvshow/1399`

### GET /api/myfavorites
Renvoie une liste de favoris pour l'utilisateur actuellement connecté.
- Exemple : `/api/myfavorites`

### GET /api/myfavorites/{id}
Renvoie une valeur spécifique.
- Paramètres : `id` (requis)
- Exemple : `/api/myfavorites/1`

### POST /api/myfavorites
Ajoute un favori pour l'utilisateur actuellement connecté.
- Paramètres : `id` (requis), `cinematography` (requis)
- Exemple : `/api/myfavorites?id=1&cinematography=movie`

### PUT /api/myfavorites/{id}
Mise à jour d'une valeur spécifique. Cette route ne semble pas être utilisée actuellement.
- Paramètres : `id` (requis), `value` (requis dans le corps de la requête)
- Exemple : `/api/myfavorites/1`

### DELETE /api/myfavorites/{id}
Supprime un favori pour l'utilisateur actuellement connecté.
- Paramètres : `id` (requis), `cinematography` (requis)
- Exemple : `/api/myfavorites?id=1&cinematography=movie`

### POST /login
Connecte un utilisateur existant.
- Paramètres : `Email`, `Password`
- Exemple : `/login`


### POST /register
Crée un nouvel utilisateur.
- Paramètres : `Email`, `Password`, `UserName`
- Exemple : `/register`