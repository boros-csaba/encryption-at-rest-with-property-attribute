# POC project - Encryption at rest using EF
A quick proof of concept for implementing encryption at rest using EF and an attribute for easily applying encryption to selected model properties. I also have a tutorial on this topic on my website: TODO add link

## How it works
The EncryptionValueConverter class will encrypt values on write and decrypts on read.
![image](https://github.com/boros-csaba/encryption-at-rest-with-property-attribute/assets/18496537/e118c475-9fd0-4855-a0b1-9cc34424be66)

The EncryptionValueConverter is applied to all the fields that have the EncryptedAttribute.
![image](https://github.com/boros-csaba/encryption-at-rest-with-property-attribute/assets/18496537/80b686bf-a988-41b9-84bb-37b3e5bfd465)


## Testing
This repository incldudes a test project you can use to see the logic in action. You will need to have docker installed on your machine because the test uses Testcontainers to create a PostreSQL instance. The test will save an entity with an encrypted field to the database and it will read it back in two different ways. When reading it back using EF the result will be the decrypted plain text value you saved. When reading it using raw SQL the result will be the encryoted cypher text that is stored in the db.
