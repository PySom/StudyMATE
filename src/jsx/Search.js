import React from "react";
import videos from '../data/data';
// import '../js/bodyonclick'


export class Search extends React.Component{
     constructor(props){
        super(props)
        this.state = { input: ''}
        this.fresh = [];
        this.searchRef = React.createRef();
       
    }

    handleClick(id){
        this.props.onClick(id);
        this.props.history.push(`/video/${id}`)
        this.searchRef.current.classList.add('d-none')
        console.log(this.searchRef)
    }

    componentDidMount(){
       this.searchRef.current.ownerDocument.body.addEventListener('click', ()=> {
           console.log('i was clicked')
           this.searchRef.current.classList.add('d-none')
       })
                    
    }
        
    
    
    videoFilter = (term) => {
        this.searchRef.current.classList.remove('d-none')
        const searchArray = [];
        // const searchFilter = []
        const videoList =  videos.filter((video) => {
             return video.Videos
         })
         
         for (let vidItem = 0; vidItem < videoList.length; vidItem++) {
             const element = videoList[vidItem];
             const innerArray = element.Videos;
            //  console.log(innerArray);
            
            for (let i = 0; i < innerArray.length; i++) {
                const element = innerArray[i];
                searchArray.push({id: element.VideoId, video: element.VideoUrl.toLocaleLowerCase()})
                
            }
           
        }
       this.fresh = searchArray.filter((vid) => {
            return vid.video.includes(term)
        }).map((item) => {
            return (
                <li key={item.id}>
                    <span onClick={() => this.handleClick(item.id)}>{item.video}</span>
                </li>
            )
        })
        console.log(this.fresh)
    }

    render(){
        return(
            <div>
               <form> 
                 <div className="form-group">
                   <input className="form-control" value={this.state.input} type="text" onKeyUp={(e) => this.videoFilter(e.target.value)} onChange={e => this.setState({ input: e.target.value.toLocaleLowerCase()})} />
                  </div>
              </form>
              <div>
                <ul className="search-results" ref={this.searchRef}>
                    {this.fresh}
                </ul>
              </div>
            </div>
        )
    }
}