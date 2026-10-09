use PostOSharePostsDb

db.createCollection("Posts")
db.Posts.createIndex({ "partitionKey": 1 })