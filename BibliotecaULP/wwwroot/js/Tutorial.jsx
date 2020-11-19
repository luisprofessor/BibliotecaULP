
/*class CommentBox extends React.Component {
    render() {
        return (<div className="commentBox">
            <h1> Comments</h1>
            <CommentList data={this.props.data} />
            <CommentForm />

        </div>


        );
    }
}

class CommentList extends React.Component {

    render() {
        const commentNodes = this.props.data.map(comment => (
            <Comment author={comment.author} key={comment.id}>
                {comment.text}
            </Comment>
        ));
        return (
            <div className="commentList">
                {commentNodes}


            </div>)
    }
}

class CommentForm extends React.Component {
    render() {
        return (<div className="commentForm"> Comment Form </div>)
    }
}

class Comment extends React.Component {
    render() {
        return (
            <div className="comment">
                <h2 className="commentAuthor">{this.props.author}</h2>
                {this.props.children}
            </div>
        );
    }
}

class Prueba extends React.Component{
   
    render()
    {
        return (
            <div className ="prueba">
                {this.props.titulo}

            </div>
            );
    }
}
const data = [
    { id: 1, author: 'Daniel Lo Nigro', text: 'Hello ReactJS.NET World!' },
    { id: 2, author: 'Pete Hunt', text: 'This is one comment' },
    { id: 3, author: 'Jordan Walke', text: 'This is *another* comment' },
];


ReactDOM.render(<CommentBox data={data} />, document.getElementById('home'));*/