# Documentation of AI Conversations

To keep things structured, provide the name of the GenAI platform, the date it was used, a copy-pasta of the question, and a summary of the response.

## CHATGPT Usage:

ChatGPT5 was used to generate a majority of the content for our developed game.

ChatGPT was used to both generate sprites and assets, and music aswell. 

## CHATGPT5 Image Generation

To generate consistent assets a project was created within the available tools found on the online website.

### Assets:

### Base Images and 'Differences'

The first phase of the game consistents of the player interacting with objects (e.g. wardrobe, bookcase, desk, etc.) in a room. Once interacting, an image appears showing the interacted object from the perspective of the playable character. Each new day there are differences in these images which the player must detect and click on. To generate these images, we first generated images of the interactable objects without any other objects/items on them. This allowed us to define the desired style for the images to ensure consistenty across all the images (once one was generated in the desired style, it was used as a reference). Additionally, sketches were provided to ChatGPT to give it a reference on the perspective we wanted

[EXAMPLE SKETCH]

[EXAMPLE GENERATED IMAGE]

[EXAMPLE PROMPTS]

Once these images where generated, we iteratived on them. By providing the model with the barren objects, we asked it to add populate the images with specific objects in specific locations. The was also done to ensure consistency across all the images and objects.

[EXAMPLE PROMPTS]

[EXAMLE ITERATIONS]

After the images were populated with items, we needed to generate the differences. To do so, we took a similar approach as above. The image (populated with objects) was provided to ChatGPT and then prompted to make a specific change. We decided to perform one difference per generation, as this would prevent overloading the model with too many tasks. We thought that asking for multiple differences in one image generation would lead inconsistencies in style and slight undesired changes. 

[EXAMPLE IMAGE]

The resulting images were fairly good in quality and consistency.
